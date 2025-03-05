using System.Collections.Generic;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Repository.Yaml.Variables.Models;

public sealed class VariableSymbolRootModel : ISymbolRootModel<VariableSymbolModel>
{
    public string FormatVersion { get; set; } = "1.0.0";
    public List<VariableSymbolModel> Data { get; set; } = [];
}
