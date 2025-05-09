using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace KSPCompiler.Shared.IO.Symbols.Yaml.Callbacks.Models;

public sealed class CallbackArgumentModel
{
    public string Name { get; set; } = string.Empty;

    public bool RequiredDeclare { get; set; }
    [YamlMember(ScalarStyle = ScalarStyle.Literal)]

    public string Description { get; set; } = string.Empty;
}
