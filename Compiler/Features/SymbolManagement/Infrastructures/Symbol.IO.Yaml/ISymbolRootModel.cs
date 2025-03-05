using System.Collections.Generic;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Repository.Yaml;

public interface ISymbolRootModel<TModel>
{
    string FormatVersion { get; set; }
    List<TModel> Data { get; set; }
}
