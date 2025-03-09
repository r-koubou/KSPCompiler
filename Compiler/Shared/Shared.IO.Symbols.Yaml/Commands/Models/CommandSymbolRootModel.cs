using System.Collections.Generic;

namespace KSPCompiler.Shared.IO.Symbols.Yaml.Commands.Models;

public sealed class CommandSymbolRootModel : ISymbolRootModel<CommandSymbolModel>
{
    public string FormatVersion { get; set; } = "1.0.0";
    public List<CommandSymbolModel> Data { get; set; } = [];
}
