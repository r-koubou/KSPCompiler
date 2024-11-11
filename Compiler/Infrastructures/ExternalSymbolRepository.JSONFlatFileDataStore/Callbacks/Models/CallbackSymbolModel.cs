using System;
using System.Collections.Generic;

using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commons.Models;

namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Callbacks.Models;

public sealed class CallbackSymbolModel : ISymbolModel
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string Name { get; set; } = string.Empty;
    public bool BuiltIn { get; set; }
    public bool AllowMultipleDeclaration { get; set; }
    public string Description { get; set; } = string.Empty;
    public string BuiltIntoVersion { get; set; } = string.Empty;
    public List<CallbackArgumentModel> Arguments { get; set; } = new();
}
