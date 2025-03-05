using System.Collections.Generic;

namespace KSPCompiler.Features.Compilation.Infrastructures.SymbolRepository.Yaml.UITypes.Models;

public sealed class UITypeSymbolRootModel : ISymbolRootModel<UITypeSymbolModel>
{
    public string FormatVersion { get; set; } = "1.0.0";
    public List<UITypeSymbolModel> Data { get; set; } = [];
}
