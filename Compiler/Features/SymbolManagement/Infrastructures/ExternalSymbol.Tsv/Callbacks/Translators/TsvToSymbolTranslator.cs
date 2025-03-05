using System.Collections.Generic;
using System.IO;

using CsvHelper;

using KSPCompiler.Features.SymbolManagement.Infrastructures.ExternalSymbol.Tsv.Callbacks.Models;
using KSPCompiler.Features.SymbolManagement.Infrastructures.ExternalSymbol.Tsv.Callbacks.Models.CsvHelperMappings;
using KSPCompiler.Shared;
using KSPCompiler.Shared.Domain.Symbols;
using KSPCompiler.Shared.Domain.Symbols.MetaData;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.ExternalSymbol.Tsv.Callbacks.Translators;

internal class TsvToSymbolTranslator : IDataTranslator<string, IReadOnlyCollection<CallbackSymbol>>
{
    public IReadOnlyCollection<CallbackSymbol> Translate( string source )
    {
        var result = new List<CallbackSymbol>();

        using var reader = new StringReader( source );
        using var csvReader = new CsvReader( reader, ConstantValue.ReaderConfiguration );

        csvReader.Context.RegisterClassMap<CallbackModelClassMap>();

        var records = csvReader.GetRecords<CallbackModel>();

        foreach( var record in records )
        {
            var symbol = new CallbackSymbol( record.AllowMultipleDeclaration )
            {
                Name             = record.Name,
                BuiltIn          = record.BuiltIn,
                Description      = record.Description,
                BuiltIntoVersion = record.BuiltIntoVersion
            };

            foreach( var argument in record.Arguments )
            {
                symbol.Arguments.Add( new CallbackArgumentSymbol( argument.RequiredDeclareOnInit )
                    {
                        Name        = argument.Name,
                        Description = argument.Description,
                        BuiltIn     = false,
                        DataType    = DataTypeUtility.GuessFromSymbolName( argument.Name )
                    }
                );
            }

            result.Add( symbol );
        }

        return result;
    }
}
