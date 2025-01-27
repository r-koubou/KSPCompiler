using System.Collections.Generic;

namespace KSPCompiler.ExternalSymbol.Yaml.UITypes.Models;

public sealed class UITypeSymbolRootModel : ISymbolRootModel<UITypeSymbolModel>
{
    public string FormatVersion { get; set; } = "1.0.0";
    public List<UITypeSymbolModel> Data { get; set; } = [];
}
