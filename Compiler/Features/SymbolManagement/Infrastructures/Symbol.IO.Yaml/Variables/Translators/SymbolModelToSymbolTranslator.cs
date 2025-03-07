using KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Repository.Yaml.Variables.Models;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Repository.Yaml.Variables.Translators;

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
