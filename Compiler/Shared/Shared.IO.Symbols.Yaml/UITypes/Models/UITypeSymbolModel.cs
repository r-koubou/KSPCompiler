using System.Collections.Generic;

using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace KSPCompiler.Shared.IO.Symbols.Yaml.UITypes.Models;

public sealed class UITypeSymbolModel
{
    public string Name { get; set; } = string.Empty;

    public bool BuiltIn { get; set; }

    public string VariableType { get; set; } = string.Empty;

    [YamlMember( ScalarStyle = ScalarStyle.Literal )]
    public string Description { get; set; } = string.Empty;

    public string BuiltIntoVersion { get; set; } = string.Empty;

    public bool RequireInitializer { get; set; }

    public List<UITypeSymbolArgumentModel> InitializerArguments { get; set; } = [];
}
