using System.Collections.Generic;

using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace KSPCompiler.Features.Compilation.Infrastructures.ExternalSymbol.Yaml.Commands.Models;

public sealed class CommandSymbolModel
{
    public string Name { get; set; } = string.Empty;

    public bool BuiltIn { get; set; } = true;

    [YamlMember( ScalarStyle = ScalarStyle.Literal )]
    public string Description { get; set; } = string.Empty;

    public string BuiltIntoVersion { get; set; } = string.Empty;

    public string ReturnType { get; set; } = string.Empty;

    public List<CommandArgumentModel> Arguments { get; set; } = [];
}
