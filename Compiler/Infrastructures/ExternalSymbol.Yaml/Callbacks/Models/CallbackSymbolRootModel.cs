using System.Collections.Generic;

namespace KSPCompiler.ExternalSymbol.Yaml.Callbacks.Models;

public class CallbackSymbolRootModel : ISymbolRootModel<CallBackSymbolModel>
{
    public string FormatVersion { get; set; } = "1.0.0";
    public List<CallBackSymbolModel> Data { get; set; } = [];
}
