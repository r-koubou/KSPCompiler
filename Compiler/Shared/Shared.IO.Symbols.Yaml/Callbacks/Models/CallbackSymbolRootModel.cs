using System.Collections.Generic;

namespace KSPCompiler.Shared.IO.Symbols.Yaml.Callbacks.Models;

public class CallbackSymbolRootModel : ISymbolRootModel<CallBackSymbolModel>
{
    public string FormatVersion { get; set; } = "1.0.0";
    public List<CallBackSymbolModel> Data { get; set; } = [];
}
