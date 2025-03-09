using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Symbols.Yaml.Variables.Models;

namespace KSPCompiler.Shared.IO.Symbols.Yaml.Variables.Translators;

public class SymbolToSymbolModelTranslator
    : ISymbolToSymbolModelTranslator<VariableSymbol, VariableSymbolModel>
{
    public VariableSymbolModel Translate( VariableSymbol source )
    {
        var symbol = new VariableSymbolModel
        {
            Name             = source.Name.Value,
            BuiltIn          = source.BuiltIn,
            Description      = source.Description.Value,
            BuiltIntoVersion = source.BuiltIntoVersion
        };

        return symbol;
    }
}
