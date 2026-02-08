using LumexUI.Cli.Models;

namespace LumexUI.Cli.Registry;

public interface IRegistryProvider
{
    Task<RegistryManifest> GetManifestAsync(CancellationToken cancellationToken = default);
    Task<byte[]> GetFileAsync(string componentName, string fileName, CancellationToken cancellationToken = default);
}
