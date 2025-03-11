using System;
using System.Collections.Generic;

using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace KSPCompiler.Shared.IO.Symbols.Yaml.Callbacks.Models;

public sealed class CallBackSymbolModel : ISymbolModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool BuiltIn { get; set; }
    public bool AllowMultipleDeclaration { get; set; }
    [YamlMember(ScalarStyle = ScalarStyle.Literal)]
    public string Description { get; set; } = string.Empty;
    public string BuiltIntoVersion { get; set; } = string.Empty;
    public List<CallbackArgumentModel> Arguments { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
