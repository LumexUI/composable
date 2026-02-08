using System.Diagnostics;

using LumexUI.Cli.Models;

namespace LumexUI.Cli.Services;

public sealed class PackageInstaller
{
	public async Task<PackageInstallResult> InstallPackagesAsync(
		IEnumerable<PackageReference> packages,
		string projectPath,
		CancellationToken cancellationToken = default )
	{
		var result = new PackageInstallResult();

		foreach( var package in packages )
		{
			var success = await InstallPackageAsync( package, projectPath, cancellationToken );

			if( success )
			{
				result.Installed.Add( package );
			}
			else
			{
				result.Failed.Add( package );
			}
		}

		return result;
	}

	private static async Task<bool> InstallPackageAsync(
		PackageReference package,
		string projectPath,
		CancellationToken cancellationToken )
	{
		var args = string.IsNullOrEmpty( package.Version )
			? $"add \"{projectPath}\" package {package.Name}"
			: $"add \"{projectPath}\" package {package.Name} --version {package.Version}";

		var startInfo = new ProcessStartInfo
		{
			FileName = "dotnet",
			Arguments = args,
			RedirectStandardOutput = true,
			RedirectStandardError = true,
			UseShellExecute = false,
			CreateNoWindow = true
		};

		using var process = new Process { StartInfo = startInfo };

		try
		{
			process.Start();
			await process.WaitForExitAsync( cancellationToken );
			return process.ExitCode == 0;
		}
		catch
		{
			return false;
		}
	}
}

public sealed class PackageInstallResult
{
	public List<PackageReference> Installed { get; } = [];
	public List<PackageReference> Failed { get; } = [];
}
