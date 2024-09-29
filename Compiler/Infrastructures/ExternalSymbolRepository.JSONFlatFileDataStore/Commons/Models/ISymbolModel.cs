using System;

namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commons.Models;

public interface ISymbolModel
{
    string Id { get; set; }
    string Name { get; set; }
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
}
