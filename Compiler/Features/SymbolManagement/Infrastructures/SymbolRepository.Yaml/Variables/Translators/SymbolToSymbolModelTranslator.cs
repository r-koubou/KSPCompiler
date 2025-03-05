using KSPCompiler.Features.SymbolManagement.Infrastructures.SymbolRepository.Yaml.Variables.Models;
using KSPCompiler.Shared.Domain.Symbols;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.SymbolRepository.Yaml.Variables.Translators;

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
