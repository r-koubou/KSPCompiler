using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables.Models;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Variables.Translators;

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
