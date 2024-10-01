using System;
using System.Collections.Generic;

using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commons.Models;

namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commands.Models;

public sealed class CommandSymbolModel : ISymbolModel
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string Name { get; set; } = string.Empty;
    public bool Reserved { get; set; } = true;
    public string Description { get; set; } = string.Empty;
    public string ReturnType { get; set; } = string.Empty;
    public List<CommandArgumentModel> Arguments { get; set; } = new();
}
