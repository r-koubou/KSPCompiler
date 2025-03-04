using System.Collections.Generic;

namespace KSPCompiler.Features.Compilation.Infrastructures.ExternalSymbol.Yaml.Variables.Models;

public sealed class VariableSymbolRootModel : ISymbolRootModel<VariableSymbolModel>
{
    public string FormatVersion { get; set; } = "1.0.0";
    public List<VariableSymbolModel> Data { get; set; } = [];
}
