using System;
using System.Collections.Generic;

namespace KSPCompiler.Features.Compilation.Infrastructures.ExternalSymbolRepository.Yaml;

public interface ISymbolRootModel<TModel> where TModel : ISymbolModel
{
    string FormatVersion { get; set; }
    List<TModel> Data { get; set; }
}

public interface ISymbolModel
{
    Guid Id { get; set; }
    string Name { get; set; }
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
}
