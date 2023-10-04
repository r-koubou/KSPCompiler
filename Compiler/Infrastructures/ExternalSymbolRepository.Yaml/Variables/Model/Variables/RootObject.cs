using System.Collections.Generic;

using YamlDotNet.Serialization;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Variables.Model.Variables;

// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
public class RootObject
{
    [YamlIgnore]
    public const int UnknownVersion = -1;

    public int Version { get; set; } = UnknownVersion;
    public List<Symbol> Symbols { get; set; } = new();
}
