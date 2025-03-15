using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Symbols.Yaml.Variables.Models;

namespace KSPCompiler.Shared.IO.Symbols.Yaml.Variables.Translators;

public sealed class SymbolToSymbolModelTranslator
    : ISymbolToSymbolModelTranslator<VariableSymbol, VariableSymbolModel>
{
    public VariableSymbolModel Translate( VariableSymbol source )
    {
        var symbol = new VariableSymbolModel
        {
            Id               = source.Id,
            CreatedAt        = source.CreatedAt,
            UpdatedAt        = source.UpdatedAt,
            Name             = source.Name.Value,
            BuiltIn          = source.BuiltIn,
            Description      = source.Description.Value,
            BuiltIntoVersion = source.BuiltIntoVersion
        };

        return symbol;
    }
}
