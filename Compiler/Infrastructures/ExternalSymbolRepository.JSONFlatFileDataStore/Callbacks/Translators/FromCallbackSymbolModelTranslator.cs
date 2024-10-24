using System.Collections.Generic;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Callbacks.Models;

namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Callbacks.Translators;

internal class FromCallbackSymbolModelTranslator : IDataTranslator<IEnumerable<CallbackSymbolModel>, IReadOnlyCollection<CallbackSymbol>>
{
    public IReadOnlyCollection<CallbackSymbol> Translate( IEnumerable<CallbackSymbolModel> source )
    {
        var result = new List<CallbackSymbol>();

        foreach( var x in source )
        {
            var command = new CallbackSymbol( x.AllowMultipleDeclaration )
            {
                Name        = x.Name,
                Reserved    = x.Reserved,
                Description = x.Description,
                DataType    = DataTypeFlag.None
            };

            foreach( var arg in x.Arguments )
            {
                var argument = new CallbackArgumentSymbol( arg.RequiredDeclare )
                {
                    Name        = arg.Name,
                    Description = arg.Description,
                    Reserved    = false
                };

                argument.DataType = DataTypeUtility.GuessFromSymbolName( argument.Name );
                command.AddArgument( argument );
            }

            result.Add( command );
        }

        return result;
    }
}
