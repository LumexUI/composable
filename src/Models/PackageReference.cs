using System.Text.Json.Serialization;

namespace LumexUI.Cli.Models;

public sealed class PackageReference
{
	[JsonPropertyName( "name" )]
	public required string Name { get; init; }

	[JsonPropertyName( "version" )]
	public string? Version { get; init; }
}
