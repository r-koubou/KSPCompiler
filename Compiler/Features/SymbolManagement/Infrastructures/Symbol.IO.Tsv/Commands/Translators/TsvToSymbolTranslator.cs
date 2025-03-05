using System.Collections.Generic;
using System.IO;

using CsvHelper;

using KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Tsv.Commands.Models;
using KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Tsv.Commands.Models.CsvHelperMappings;
using KSPCompiler.Shared;
using KSPCompiler.Shared.Domain.Symbols;
using KSPCompiler.Shared.Domain.Symbols.MetaData;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Tsv.Commands.Translators;

public class TsvToSymbolTranslator : IDataTranslator<string, IReadOnlyCollection<CommandSymbol>>
{
    public IReadOnlyCollection<CommandSymbol> Translate( string source )
    {
        var result = new List<CommandSymbol>();

        using var reader = new StringReader( source );
        using var csvReader = new CsvReader( reader, ConstantValue.ReaderConfiguration );

        csvReader.Context.RegisterClassMap<CommandModelClassMap>();

        var records = csvReader.GetRecords<CommandModel>();

        foreach( var record in records )
        {
            var symbol = new CommandSymbol
            {
                Name             = record.Name,
                BuiltIn          = record.BuiltIn,
                Description      = record.Description,
                BuiltIntoVersion = record.BuiltIntoVersion,
                DataType         = DataTypeUtility.GuessFromTypeString( record.ReturnType )
            };

            foreach( var argument in record.Arguments )
            {
                var uiTypeNames = new List<string>();
                var otherTypeNames = new List<string>();

                DataTypeUtility.GuessFromTypeString( argument.DataType, out var typeFlag, ref uiTypeNames, ref otherTypeNames );

                symbol.AddArgument( new CommandArgumentSymbol( uiTypeNames, otherTypeNames )
                    {
                        Name        = argument.Name,
                        Description = argument.Description,
                        BuiltIn     = false,
                        DataType    = typeFlag
                    }
                );
            }

            result.Add( symbol );
        }

        return result;
    }
}
