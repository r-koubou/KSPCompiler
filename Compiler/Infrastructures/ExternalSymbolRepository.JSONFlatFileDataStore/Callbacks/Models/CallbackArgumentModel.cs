namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Callbacks.Models;

public class CallbackArgumentModel
{
    public string Name { get; set; } = string.Empty;
    public bool RequiredDeclare { get; set; }
    public string Description { get; set; } = string.Empty;
}
