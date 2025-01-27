using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.UITypes.Models;

public sealed class UITypeSymbolArgumentModel
{
    public string Name { get; set; } = string.Empty;

    [YamlMember( ScalarStyle = ScalarStyle.Literal )]
    public string Description { get; set; } = string.Empty;
}
