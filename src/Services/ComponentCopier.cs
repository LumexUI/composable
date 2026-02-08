using LumexUI.Cli.Models;
using LumexUI.Cli.Registry;

namespace LumexUI.Cli.Services;

public sealed class ComponentCopier
{
    private readonly IRegistryProvider _registryProvider;

    public ComponentCopier(IRegistryProvider registryProvider)
    {
        _registryProvider = registryProvider;
    }

    public async Task<CopyResult> CopyComponentsAsync(
        IEnumerable<string> componentNames,
        string outputDir,
        CancellationToken cancellationToken = default)
    {
        var manifest = await _registryProvider.GetManifestAsync(cancellationToken);
        var componentMap = manifest.Components.ToDictionary(c => c.Name, StringComparer.OrdinalIgnoreCase);

        var result = new CopyResult();

        foreach (var componentName in componentNames)
        {
            if (!componentMap.TryGetValue(componentName, out var component))
            {
                result.NotFound.Add(componentName);
                continue;
            }

            await CopyComponentAsync(component, outputDir, result, cancellationToken);
        }

        return result;
    }

    private async Task CopyComponentAsync(
        ComponentEntry component,
        string outputDir,
        CopyResult result,
        CancellationToken cancellationToken)
    {
        var componentDir = Path.Combine(outputDir, component.Name);
        Directory.CreateDirectory(componentDir);

        foreach (var fileName in component.Files)
        {
            var fileContent = await _registryProvider.GetFileAsync(component.Name, fileName, cancellationToken);
            var targetPath = Path.Combine(componentDir, fileName);

            await File.WriteAllBytesAsync(targetPath, fileContent, cancellationToken);

            result.CopiedFiles.Add(targetPath);
        }

        result.CopiedComponents.Add(component.Name);

        foreach (var package in component.Packages)
        {
            result.RequiredPackages.Add(package);
        }
    }
}

public sealed class CopyResult
{
    public List<string> CopiedComponents { get; } = [];
    public List<string> CopiedFiles { get; } = [];
    public List<string> NotFound { get; } = [];
    public List<PackageReference> RequiredPackages { get; } = [];
}
