using System.Collections.Generic;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Callbacks.Models;

namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Callbacks.Translators;

internal class ToCallbackSymbolModelTranslator : IDataTranslator<IEnumerable<CallbackSymbol>, IReadOnlyCollection<CallbackSymbolModel>>
{
    public IReadOnlyCollection<CallbackSymbolModel> Translate( IEnumerable<CallbackSymbol> source )
    {
        var result = new List<CallbackSymbolModel>();

        foreach( var x in source )
        {
            var symbol = new CallbackSymbolModel
            {
                Name                     = x.Name.Value,
                BuiltIn                 = x.BuiltIn,
                AllowMultipleDeclaration = x.AllowMultipleDeclaration,
                Description              = x.Description.Value,
                BuiltIntoVersion         = x.BuiltIntoVersion
            };

            foreach( var arg in x.Arguments )
            {
                var argument = new CallbackArgumentModel
                {
                    Name            = arg.Name,
                    RequiredDeclare = arg.RequiredDeclareOnInit,
                    Description     = arg.Description
                };

                symbol.Arguments.Add( argument );
            }

            result.Add( symbol );
        }

        return result;
    }
}
