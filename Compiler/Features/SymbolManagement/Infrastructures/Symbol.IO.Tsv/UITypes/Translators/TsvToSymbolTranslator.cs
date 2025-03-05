using System.Collections.Generic;
using System.IO;

using CsvHelper;

using KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Tsv.UITypes.Models;
using KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Tsv.UITypes.Models.CsvHelperMappings;
using KSPCompiler.Shared;
using KSPCompiler.Shared.Domain.Symbols;
using KSPCompiler.Shared.Domain.Symbols.MetaData;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Tsv.UITypes.Translators;

public class TsvToSymbolTranslator : IDataTranslator<string, IReadOnlyCollection<UITypeSymbol>>
{
    public IReadOnlyCollection<UITypeSymbol> Translate( string source )
    {
        var result = new List<UITypeSymbol>();

        using var reader = new StringReader( source );
        using var csvReader = new CsvReader( reader, ConstantValue.ReaderConfiguration );

        csvReader.Context.RegisterClassMap<UITypeModelClassMap>();

        var records = csvReader.GetRecords<UITypeModel>();

        foreach( var record in records )
        {
            var symbol = new UITypeSymbol( record.InitializerRequired )
            {
                Name             = record.Name,
                BuiltIn          = record.BuiltIn,
                Description      = record.Description,
                BuiltIntoVersion = record.BuiltIntoVersion,
                DataType         = DataTypeUtility.GuessFromTypeString( record.DataType )
            };

            foreach( var argument in record.Arguments )
            {
                var typeFlag = DataTypeUtility.GuessFromSymbolName( argument.Name );

                symbol.AddInitializerArgument( new UIInitializerArgumentSymbol
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
