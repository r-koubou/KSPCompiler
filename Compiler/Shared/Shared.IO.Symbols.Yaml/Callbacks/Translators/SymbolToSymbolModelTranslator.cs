using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Symbols.Yaml.Callbacks.Models;

namespace KSPCompiler.Shared.IO.Symbols.Yaml.Callbacks.Translators;

public sealed class SymbolToSymbolModelTranslator
    : ISymbolToSymbolModelTranslator<CallbackSymbol, CallBackSymbolModel>
{
    public CallBackSymbolModel Translate( CallbackSymbol source )
    {
        var symbol = new CallBackSymbolModel
        {
            Id                       = source.Id,
            CreatedAt                = source.CreatedAt,
            UpdatedAt                = source.UpdatedAt,
            Name                     = source.Name.Value,
            BuiltIn                  = source.BuiltIn,
            AllowMultipleDeclaration = source.AllowMultipleDeclaration,
            Description              = source.Description.Value,
            BuiltIntoVersion         = source.BuiltIntoVersion
        };

        foreach( var arg in source.Arguments )
        {
            var argument = new CallbackArgumentModel
            {
                Name            = arg.Name,
                RequiredDeclare = arg.RequiredDeclareOnInit,
                Description     = arg.Description
            };

            symbol.Arguments.Add( argument );
        }

        return symbol;
    }
}
