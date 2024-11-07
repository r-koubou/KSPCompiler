using System.Collections.Generic;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commands.Models;

namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commands.Translators;

internal class ToCommandSymbolModelTranslator : IDataTranslator<IEnumerable<CommandSymbol>, IReadOnlyCollection<CommandSymbolModel>>
{
    public IReadOnlyCollection<CommandSymbolModel> Translate( IEnumerable<CommandSymbol> source )
    {
        var result = new List<CommandSymbolModel>();

        foreach( var x in source )
        {
            var symbol = new CommandSymbolModel
            {
                Name             = x.Name.Value,
                Reserved         = x.Reserved,
                ReturnType       = DataTypeUtility.ToString( x.DataType ),
                Description      = x.Description.Value,
                BuiltIntoVersion = x.BuiltIntoVersion
            };

            foreach( var arg in x.Arguments )
            {
                var argument = new CommandArgumentModel
                {
                    Name        = arg.Name,
                    Description = arg.Description
                };

                symbol.Arguments.Add( argument );
            }

            result.Add( symbol );
        }

        return result;
    }
}
