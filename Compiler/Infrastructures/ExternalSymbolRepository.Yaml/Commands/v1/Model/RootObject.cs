using System.Collections.Generic;

using YamlDotNet.Serialization;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Commands.v1.Model;

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

public sealed class Symbol
{
    public string Name { get; set; } = string.Empty;
    public bool Reserved { get; set; } = true;
    public string Description { get; set; } = string.Empty;
    public string ReturnType { get; set; } = string.Empty;
    public List<Argument> Arguments { get; set; } = new();
}

public sealed class Argument
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
