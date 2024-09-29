using System;

using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commons.Models;

namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Variables.Models;

public sealed class VariableSymbolModel : ISymbolModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string Name { get; set; } = string.Empty;
    public bool Reserved { get; set; } = true;
    public string Description { get; set; } = string.Empty;
}
