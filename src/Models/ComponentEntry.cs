using System.Text.Json.Serialization;

namespace LumexUI.Cli.Models;

public sealed class ComponentEntry
{
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("files")]
    public required string[] Files { get; init; }

    [JsonPropertyName("dependencies")]
    public string[] Dependencies { get; init; } = [];

    [JsonPropertyName("packages")]
    public PackageReference[] Packages { get; init; } = [];
}
