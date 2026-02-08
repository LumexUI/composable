using System.Text.Json.Serialization;

namespace LumexUI.Cli.Models;

public sealed class RegistryManifest
{
    [JsonPropertyName("components")]
    public required ComponentEntry[] Components { get; init; }
}
