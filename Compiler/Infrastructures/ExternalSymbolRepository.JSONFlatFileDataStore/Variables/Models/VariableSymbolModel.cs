using System;

using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commons.Models;

namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Variables.Models;

public sealed class VariableSymbolModel : ISymbolModel
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string Name { get; set; } = string.Empty;
    public bool BuiltIn { get; set; } = true;
    public string Description { get; set; } = string.Empty;
    public string BuiltIntoVersion { get; set; } = string.Empty;
}
