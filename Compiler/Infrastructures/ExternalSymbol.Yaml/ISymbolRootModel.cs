using System.Collections.Generic;

namespace KSPCompiler.ExternalSymbol.Yaml;

public interface ISymbolRootModel<TModel>
{
    string FormatVersion { get; set; }
    List<TModel> Data { get; set; }
}
