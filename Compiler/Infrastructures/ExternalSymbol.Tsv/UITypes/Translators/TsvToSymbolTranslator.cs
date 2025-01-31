using System.Collections.Generic;
using System.IO;

using CsvHelper;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.ExternalSymbol.Tsv.UITypes.Models;
using KSPCompiler.ExternalSymbol.Tsv.UITypes.Models.CsvHelperMappings;

namespace KSPCompiler.ExternalSymbol.Tsv.UITypes.Translators;

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
                DataType         = DataTypeUtility.GuessFromTypeString( record.DataType ),
                Description      = record.Description,
                BuiltIntoVersion = record.BuiltIntoVersion,
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
