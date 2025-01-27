using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbol.Yaml.Callbacks.Models;

namespace KSPCompiler.ExternalSymbol.Yaml.Callbacks.Translators;

public class SymbolToSymbolModelTranslator
    : ISymbolToSymbolModelTranslator<CallbackSymbol, CallBackSymbolModel>
{
    public CallBackSymbolModel Translate( CallbackSymbol source )
    {
        var symbol = new CallBackSymbolModel
        {
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
