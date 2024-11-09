namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commands.Models;

public sealed class CommandArgumentModel
{
    public string DataType { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
