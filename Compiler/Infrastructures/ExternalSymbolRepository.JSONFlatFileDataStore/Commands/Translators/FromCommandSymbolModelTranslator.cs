using System.Collections.Generic;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commands.Models;

namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commands.Translators;

internal class FromCommandSymbolModelTranslator : IDataTranslator<IEnumerable<CommandSymbolModel>, IReadOnlyCollection<CommandSymbol>>
{
    public IReadOnlyCollection<CommandSymbol> Translate( IEnumerable<CommandSymbolModel> source )
    {
        var result = new List<CommandSymbol>();

        foreach( var x in source )
        {
            var command = new CommandSymbol
            {
                Name             = x.Name,
                Reserved         = x.Reserved,
                DataType         = DataTypeUtility.GuessFromTypeString( x.ReturnType ),
                Description      = x.Description,
                BuiltIntoVersion = x.BuiltIntoVersion
            };

            foreach( var arg in x.Arguments )
            {
                var argument = new CommandArgumentSymbol
                {
                    Name        = arg.Name,
                    Reserved    = false,
                    Description = arg.Description,
                };

                argument.DataType = DataTypeUtility.GuessFromTypeString( arg.DataType );
                command.AddArgument( argument );
            }

            result.Add( command );
        }

        return result;
    }
}
