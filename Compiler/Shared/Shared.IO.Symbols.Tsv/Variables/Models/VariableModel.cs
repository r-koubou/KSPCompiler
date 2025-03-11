namespace KSPCompiler.Shared.IO.Symbols.Tsv.Variables.Models;

public sealed class VariableModel
{
    public string Name { get; set; } = string.Empty;
    public bool BuiltIn { get; set; } = true;
    public string Description { get; set; } = string.Empty;
    public string BuiltIntoVersion { get; set; } = ConstantValue.DefaultBuiltIntoVersion;
}
