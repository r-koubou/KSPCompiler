using System.Collections.Generic;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.SymbolRepository.Yaml.UITypes.Models;

public sealed class UITypeSymbolRootModel : ISymbolRootModel<UITypeSymbolModel>
{
    public string FormatVersion { get; set; } = "1.0.0";
    public List<UITypeSymbolModel> Data { get; set; } = [];
}
