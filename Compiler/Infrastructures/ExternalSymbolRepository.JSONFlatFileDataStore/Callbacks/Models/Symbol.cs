using System;
using System.Collections.Generic;

using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commons.Models;

namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Callbacks.Models;

public class Symbol : IModelBase
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string Name { get; set; } = string.Empty;
    public bool Reserved { get; set; }
    public bool AllowMultipleDeclaration { get; set; }
    public string Description { get; set; } = string.Empty;
    public List<Argument> Arguments { get; set; } = new();
}
