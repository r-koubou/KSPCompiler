using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Repository.Yaml.Variables.Models;

public sealed class VariableSymbolModel
{
    public string Name { get; set; } = string.Empty;

    public bool BuiltIn { get; set; } = true;

    [YamlMember( ScalarStyle = ScalarStyle.Literal )]
    public string Description { get; set; } = string.Empty;

    public string BuiltIntoVersion { get; set; } = string.Empty;
}
