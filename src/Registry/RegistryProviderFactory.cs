namespace LumexUI.Cli.Registry;

public static class RegistryProviderFactory
{
    public const string DefaultRegistry = "https://raw.githubusercontent.com/LumexUI/composable/main";

    public static IRegistryProvider Create(string? registry)
    {
        var source = registry ?? DefaultRegistry;

        if (Uri.TryCreate(source, UriKind.Absolute, out var uri) &&
            (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
        {
            return new HttpRegistryProvider(source);
        }

        return new LocalRegistryProvider(source);
    }
}
