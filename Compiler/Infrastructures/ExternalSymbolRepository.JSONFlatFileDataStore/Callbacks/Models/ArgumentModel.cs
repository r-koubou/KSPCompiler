namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Callbacks.Models;

public class ArgumentModel
{
    public string Name { get; set; } = string.Empty;
    public bool RequiredDeclare { get; set; }
    public string Description { get; set; } = string.Empty;
}
