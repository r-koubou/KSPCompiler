namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Callbacks.Models;

public sealed class CallbackArgumentModel
{
    public string Name { get; set; } = string.Empty;
    public bool RequiredDeclare { get; set; }
    public string Description { get; set; } = string.Empty;
}
