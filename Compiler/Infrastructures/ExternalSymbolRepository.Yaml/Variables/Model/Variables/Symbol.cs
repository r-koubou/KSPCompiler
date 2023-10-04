using System.Collections.Generic;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Variables.Model.Variables;

// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
public class Symbol
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool Reserved { get; set; } = true;
    public string Return { get; set; } = string.Empty;
    public List<string> Args { get; set; } = new();
    public string Description { get; set; } = string.Empty;
}
