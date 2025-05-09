using System.Collections.Generic;

namespace KSPCompiler.Shared.IO.Symbols.Tsv.Callbacks.Models;

public sealed class CallbackModel
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool BuiltIn { get; set; } = true;
    public bool AllowMultipleDeclaration { get; set; }
    public string BuiltIntoVersion { get; set; } = ConstantValue.DefaultBuiltIntoVersion;
    public string Description { get; set; } = string.Empty;
    public List<CallbackArgumentModel> Arguments { get; set; } = [];
}

public class CallbackArgumentModel
{
    public string Name { get; set; } = string.Empty;
    public bool RequiredDeclareOnInit { get; set; } = true;
    public string Description { get; set; } = string.Empty;
}
