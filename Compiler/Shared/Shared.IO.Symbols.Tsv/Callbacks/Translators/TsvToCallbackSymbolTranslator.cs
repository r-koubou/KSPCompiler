using System.Collections.Generic;
using System.IO;

using CsvHelper;

using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;
using KSPCompiler.Shared.IO.Symbols.Tsv.Callbacks.Models;
using KSPCompiler.Shared.IO.Symbols.Tsv.Callbacks.Models.CsvHelperMappings;

namespace KSPCompiler.Shared.IO.Symbols.Tsv.Callbacks.Translators;

public sealed class TsvToCallbackSymbolTranslator : IDataTranslator<string, IReadOnlyCollection<CallbackSymbol>>
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
