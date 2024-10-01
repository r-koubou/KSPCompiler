using System;
using System.Collections.Generic;

using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commons.Models;

namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.UITypes.Models;

public sealed class UITypeSymbolModel : ISymbolModel
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string Name { get; set; } = string.Empty;
    public bool Reserved { get; set; }
    public string VariableType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool RequireInitializer { get; set; }
    public List<UITypeSymbolArgumentModel> InitializerArguments { get; set; } = new();
}
