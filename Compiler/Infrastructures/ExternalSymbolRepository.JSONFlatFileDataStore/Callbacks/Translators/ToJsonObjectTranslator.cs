using System.Collections.Generic;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Callbacks.Models;

namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Callbacks.Translators;

internal class ToJsonObjectTranslator : IDataTranslator<IEnumerable<CallbackSymbol>, IEnumerable<Symbol>>
{
    public IEnumerable<Symbol> Translate( IEnumerable<CallbackSymbol> source )
    {
        var result = new List<Symbol>();

        foreach( var x in source )
        {
            var symbol = new Symbol
            {
                Name        = x.Name.Value,
                Reserved    = x.Reserved,
                AllowMultipleDeclaration  = x.AllowMultipleDeclaration,
                Description = x.Description.Value
            };

            foreach( var arg in x.Arguments )
            {
                var argument = new Argument
                {
                    Name        = arg.Name,
                    RequiredDeclare = arg.RequiredDeclareOnInit,
                    Description = arg.Description
                };

                symbol.Arguments.Add( argument );
            }

            result.Add( symbol );
        }

        return result;
    }
}
