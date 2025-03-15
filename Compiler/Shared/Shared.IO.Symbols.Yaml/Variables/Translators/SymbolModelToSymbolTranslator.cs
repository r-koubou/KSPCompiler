using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;
using KSPCompiler.Shared.IO.Symbols.Yaml.Variables.Models;

namespace KSPCompiler.Shared.IO.Symbols.Yaml.Variables.Translators;

public sealed class SymbolModelToSymbolTranslator
    : ISymbolModelToSymbolTranslator<VariableSymbolModel, VariableSymbol>
{
    public VariableSymbol Translate( VariableSymbolModel source )
    {
        var symbol = new VariableSymbol
        {
            Id               = source.Id,
            CreatedAt        = source.CreatedAt,
            UpdatedAt        = source.UpdatedAt,
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
