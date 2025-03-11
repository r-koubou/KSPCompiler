using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace KSPCompiler.Shared.IO.Symbols.Yaml.Commands.Models;

public sealed class CommandArgumentModel
{
    public string Name { get; set; } = string.Empty;

    public string DataType { get; set; } = string.Empty;

    [YamlMember( ScalarStyle = ScalarStyle.Literal )]
    public string Description { get; set; } = string.Empty;
}
