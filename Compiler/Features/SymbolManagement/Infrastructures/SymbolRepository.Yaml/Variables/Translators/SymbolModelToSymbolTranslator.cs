using KSPCompiler.Features.SymbolManagement.Infrastructures.SymbolRepository.Yaml.Variables.Models;
using KSPCompiler.Shared.Domain.Symbols;
using KSPCompiler.Shared.Domain.Symbols.MetaData;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.SymbolRepository.Yaml.Variables.Translators;

public class SymbolModelToSymbolTranslator
    : ISymbolModelToSymbolTranslator<VariableSymbolModel, VariableSymbol>
{
    public VariableSymbol Translate( VariableSymbolModel source )
    {
        var symbol = new VariableSymbol
        {
            Name             = source.Name,
            ArraySize        = 0,
            BuiltIn          = source.BuiltIn,
            Description      = source.Description,
            BuiltIntoVersion = source.BuiltIntoVersion
        };

        symbol.DataType = DataTypeUtility.GuessFromSymbolName( symbol.Name );
        symbol.Modifier = ModifierFlag.Const;

        return symbol;
    }
}
