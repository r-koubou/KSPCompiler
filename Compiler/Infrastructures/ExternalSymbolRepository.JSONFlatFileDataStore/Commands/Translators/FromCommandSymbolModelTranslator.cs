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
                var uiType = new List<string>();
                var otherType = new List<string>();

                DataTypeUtility.GuessFromTypeString( arg.DataType, out var dataType, ref uiType, ref otherType );

                var argument = new CommandArgumentSymbol( uiType, otherType )
                {
                    Name        = arg.Name,
                    DataType    = dataType,
                    Reserved    = false,
                    Description = arg.Description,
                };

                command.AddArgument( argument );
            }

            result.Add( command );
        }

        return result;
    }
}
