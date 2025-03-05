using System.Collections.Generic;

using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Repository.Yaml.Callbacks.Models;

public sealed class CallBackSymbolModel
{
    public string Name { get; set; } = string.Empty;
    public bool BuiltIn { get; set; }
    public bool AllowMultipleDeclaration { get; set; }
    [YamlMember(ScalarStyle = ScalarStyle.Literal)]
    public string Description { get; set; } = string.Empty;
    public string BuiltIntoVersion { get; set; } = string.Empty;
    public List<CallbackArgumentModel> Arguments { get; set; } = new();
}
