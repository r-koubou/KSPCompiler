using System.Text;

using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;
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
            Name                     = source.Name.Value,
            BuiltIn                  = source.BuiltIn,
            AllowMultipleDeclaration = source.AllowMultipleDeclaration,
            Description              = source.Description.Value,
            BuiltIntoVersion         = source.BuiltIntoVersion
        };

        var stringBuilder = new StringBuilder();

        foreach( var arg in source.Arguments )
        {
            stringBuilder.Clear();
            DataTypeUtility.ToDataTypeString( stringBuilder, arg );

            var argument = new CallbackArgumentModel
            {
                Name            = arg.Name,
                DataType        = stringBuilder.ToString(),
                RequiredDeclare = arg.RequiredDeclareOnInit,
                Description     = arg.Description
            };

            symbol.Arguments.Add( argument );
        }

        return symbol;
    }
}
