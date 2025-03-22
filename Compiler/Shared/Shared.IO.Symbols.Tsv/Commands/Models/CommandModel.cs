using System.Collections.Generic;

namespace KSPCompiler.Shared.IO.Symbols.Tsv.Commands.Models;

public sealed class CommandModel
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool BuiltIn { get; set; } = true;
    public string BuiltIntoVersion { get; set; } = ConstantValue.DefaultBuiltIntoVersion;
    public string Description { get; set; } = string.Empty;
    public string ReturnType { get; set; } = string.Empty;
    public List<CommandArgumentModel> Arguments { get; set; } = [];
}

public sealed class CommandArgumentModel
{
    public string Name { set; get; } = string.Empty;
    public string DataType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
