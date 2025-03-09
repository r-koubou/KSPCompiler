using System.Collections.Generic;

namespace KSPCompiler.Shared.IO.Symbols.Yaml;

public interface ISymbolRootModel<TModel>
{
    string FormatVersion { get; set; }
    List<TModel> Data { get; set; }
}
