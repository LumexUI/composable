using System.CommandLine;

using LumexUI.Cli.Registry;
using LumexUI.Cli.Services;

namespace LumexUI.Cli.Commands;

public static class AddCommand
{
	public static Command Create()
	{
		var componentsArg = new Argument<string[]>( "components" )
		{
			Arity = ArgumentArity.OneOrMore,
		};

		var outputOption = new Option<string>( "--output", "-o" )
		{
			Description = "Target directory for components",
			Required = true
		};

		var registryOption = new Option<string?>( "--registry", "-r" )
		{
			Description = "Override registry source (local path or URL)"
		};

		var command = new Command( "add", "Add components to your project" )
		{
			componentsArg,
			outputOption,
			registryOption
		};

		command.SetAction( async ( parseResult, cancellationToken ) =>
		{
			var components = parseResult.GetValue( componentsArg )!;
			var output = parseResult.GetValue( outputOption )!;
			var registry = parseResult.GetValue( registryOption );

			await ExecuteAsync( components, output, registry, cancellationToken );
		} );

		return command;
	}

	private static async Task ExecuteAsync(
		string[] components,
		string output,
		string? registry,
		CancellationToken cancellationToken )
	{
		var provider = RegistryProviderFactory.Create( registry );
		var copier = new ComponentCopier( provider );

		try
		{
			Console.WriteLine( $"Adding components from: {registry ?? RegistryProviderFactory.DefaultRegistry}" );
			Console.WriteLine();

			var result = await copier.CopyComponentsAsync( components, output, cancellationToken );

			if( result.CopiedComponents.Count > 0 )
			{
				Console.WriteLine( "Added components:" );
				foreach( var component in result.CopiedComponents )
				{
					Console.WriteLine( $"  + {component}" );
				}
				Console.WriteLine();
				Console.WriteLine( $"Files written to: {Path.GetFullPath( output )}" );
			}

			if( result.NotFound.Count > 0 )
			{
				Console.WriteLine();
				Console.WriteLine( "Components not found:" );
				foreach( var name in result.NotFound )
				{
					Console.WriteLine( $"  ? {name}" );
				}
			}

			if( result.RequiredPackages.Count > 0 )
			{
				Console.WriteLine();

				var projectFile = FindProjectFile();

				if( projectFile is null )
				{
					Console.WriteLine( "Required NuGet packages (no .csproj found, install manually):" );
					foreach( var package in result.RequiredPackages )
					{
						var version = string.IsNullOrEmpty( package.Version ) ? "" : $" ({package.Version})";
						Console.WriteLine( $"  - {package.Name}{version}" );
					}
				}
				else
				{
					Console.WriteLine( "Installing NuGet packages..." );

					var installer = new PackageInstaller();
					var installResult = await installer.InstallPackagesAsync(
						result.RequiredPackages,
						projectFile,
						cancellationToken );

					if( installResult.Installed.Count > 0 )
					{
						Console.WriteLine( "Installed packages:" );
						foreach( var package in installResult.Installed )
						{
							var version = string.IsNullOrEmpty( package.Version ) ? "" : $" ({package.Version})";
							Console.WriteLine( $"  + {package.Name}{version}" );
						}
					}

					if( installResult.Failed.Count > 0 )
					{
						Console.WriteLine( "Failed to install:" );
						foreach( var package in installResult.Failed )
						{
							Console.WriteLine( $"  ! {package.Name}" );
						}
					}
				}
			}
		}
		catch( Exception ex )
		{
			Console.Error.WriteLine( $"Error: {ex.Message}" );
			Environment.ExitCode = 1;
		}
		finally
		{
			( provider as IDisposable )?.Dispose();
		}
	}

	private static string? FindProjectFile()
	{
		var csprojFiles = Directory.GetFiles( Directory.GetCurrentDirectory(), "*.csproj" );
		return csprojFiles.Length == 1 ? csprojFiles[0] : null;
	}
}
