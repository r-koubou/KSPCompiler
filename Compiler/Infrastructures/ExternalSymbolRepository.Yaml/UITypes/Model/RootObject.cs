using System.Collections.Generic;

using YamlDotNet.Serialization;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.UITypes.Model;

// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
public class RootObject
{
    [YamlIgnore]
    public const int CurrentVersion = 1;

    public int Version { get; set; } = CurrentVersion;
    public List<Symbol> Symbols { get; set; } = new();
}

public class Symbol
{
    public string Name { get; set; } = string.Empty;
    public bool Reserved { get; set; }
    public string VariableType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool RequireInitializer { get; set; }
    public List<InitializerArgument> InitializerArguments { get; set; } = new();
}

public class InitializerArgument
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
