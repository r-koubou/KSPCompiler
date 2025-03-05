using System.Collections.Generic;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.SymbolRepository.Yaml;

public interface ISymbolRootModel<TModel>
{
    string FormatVersion { get; set; }
    List<TModel> Data { get; set; }
}
