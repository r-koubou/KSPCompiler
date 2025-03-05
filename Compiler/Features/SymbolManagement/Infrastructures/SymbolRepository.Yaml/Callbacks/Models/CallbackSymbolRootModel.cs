using System.Collections.Generic;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.SymbolRepository.Yaml.Callbacks.Models;

public class CallbackSymbolRootModel : ISymbolRootModel<CallBackSymbolModel>
{
    public string FormatVersion { get; set; } = "1.0.0";
    public List<CallBackSymbolModel> Data { get; set; } = [];
}
