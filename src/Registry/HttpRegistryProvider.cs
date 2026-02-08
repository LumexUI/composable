using System.Text.Json;

using LumexUI.Cli.Models;

namespace LumexUI.Cli.Registry;

public sealed class HttpRegistryProvider : IRegistryProvider, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public HttpRegistryProvider(string baseUrl)
    {
        _baseUrl = baseUrl.TrimEnd('/');
        _httpClient = new HttpClient();
    }

    public async Task<RegistryManifest> GetManifestAsync(CancellationToken cancellationToken = default)
    {
        var url = $"{_baseUrl}/registry.json";

        using var response = await _httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var manifest = await JsonSerializer.DeserializeAsync<RegistryManifest>(stream, cancellationToken: cancellationToken);

        return manifest ?? throw new InvalidOperationException("Failed to deserialize registry manifest.");
    }

    public async Task<byte[]> GetFileAsync(string componentName, string fileName, CancellationToken cancellationToken = default)
    {
        var url = $"{_baseUrl}/Registry/{componentName}/{fileName}";

        using var response = await _httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsByteArrayAsync(cancellationToken);
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}
