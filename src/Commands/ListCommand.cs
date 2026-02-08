using System.CommandLine;

using LumexUI.Cli.Registry;

namespace LumexUI.Cli.Commands;

public static class ListCommand
{
	public static Command Create()
	{
		var registryOption = new Option<string?>( "--registry", "-r" )
		{
			Description = "Override registry source (local path or URL)"
		};

		var command = new Command( "list", "List available components" )
		{
			registryOption
		};

		command.SetAction( async ( parseResult, cancellationToken ) =>
		{
			var registry = parseResult.GetValue( registryOption );
			await ExecuteAsync( registry, cancellationToken );
		} );

		return command;
	}

	private static async Task ExecuteAsync( string? registry, CancellationToken cancellationToken )
	{
		var provider = RegistryProviderFactory.Create( registry );

		try
		{
			Console.WriteLine( $"Registry: {registry ?? RegistryProviderFactory.DefaultRegistry}" );
			Console.WriteLine();

			var manifest = await provider.GetManifestAsync( cancellationToken );

			Console.WriteLine( "Available components:" );
			Console.WriteLine();

			foreach( var component in manifest.Components.OrderBy( c => c.Name ) )
			{
				Console.WriteLine( $"  {component.Name}" );

				if( component.Files.Length > 0 )
				{
					Console.WriteLine( $"    Files: {string.Join( ", ", component.Files )}" );
				}

				if( component.Dependencies.Length > 0 )
				{
					Console.WriteLine( $"    Dependencies: {string.Join( ", ", component.Dependencies )}" );
				}
			}

			Console.WriteLine();
			Console.WriteLine( $"Total: {manifest.Components.Length} components" );
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
}
