using System.Text.Json;

using LumexUI.Cli.Models;

namespace LumexUI.Cli.Registry;

public sealed class LocalRegistryProvider : IRegistryProvider
{
    private readonly string _basePath;

    public LocalRegistryProvider(string basePath)
    {
        _basePath = Path.GetFullPath(basePath);
    }

    public async Task<RegistryManifest> GetManifestAsync(CancellationToken cancellationToken = default)
    {
        var manifestPath = Path.Combine(_basePath, "registry.json");

        if (!File.Exists(manifestPath))
        {
            throw new FileNotFoundException($"Registry manifest not found at: {manifestPath}");
        }

        await using var stream = File.OpenRead(manifestPath);
        var manifest = await JsonSerializer.DeserializeAsync<RegistryManifest>(stream, cancellationToken: cancellationToken);

        return manifest ?? throw new InvalidOperationException("Failed to deserialize registry manifest.");
    }

    public async Task<byte[]> GetFileAsync(string componentName, string fileName, CancellationToken cancellationToken = default)
    {
        var filePath = Path.Combine(_basePath, "components", componentName, fileName);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Component file not found: {filePath}");
        }

        return await File.ReadAllBytesAsync(filePath, cancellationToken);
    }
}
