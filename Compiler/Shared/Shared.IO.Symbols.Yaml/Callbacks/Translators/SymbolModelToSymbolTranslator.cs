using System.Collections.Generic;

using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;
using KSPCompiler.Shared.IO.Symbols.Yaml.Callbacks.Models;

namespace KSPCompiler.Shared.IO.Symbols.Yaml.Callbacks.Translators;

public sealed class SymbolModelToSymbolTranslator
    : ISymbolModelToSymbolTranslator<CallBackSymbolModel, CallbackSymbol>
{
    public CallbackSymbol Translate( CallBackSymbolModel source )
    {
        var model = new CallbackSymbol( source.AllowMultipleDeclaration )
        {
            Id               = source.Id,
            Name             = source.Name,
            BuiltIn          = source.BuiltIn,
            Description      = source.Description,
            BuiltIntoVersion = source.BuiltIntoVersion
        };

        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        foreach( var arg in source.Arguments )
        {
            var uiType = new List<string>();
            var otherType = new List<string>();

            DataTypeUtility.GuessFromTypeString( arg.DataType, out var dataType, ref uiType, ref otherType );

            var argument = new CallbackArgumentSymbol( arg.RequiredDeclare, uiType, otherType )
            {
                Name        = arg.Name,
                DataType    = dataType,
                Description = arg.Description,
                BuiltIn     = false
            };

            model.Arguments.Add( argument );
        }

        return model;
    }
}
