using System.Collections.Generic;
using System.IO;

using CsvHelper;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.ExternalSymbol.Tsv.UITypes.Models.CsvHelperMappings;
using KSPCompiler.ExternalSymbol.Tsv.Variables.Models;

namespace KSPCompiler.ExternalSymbol.Tsv.Variables.Translators;

public class TsvToSymbolTranslator : IDataTranslator<string, IReadOnlyCollection<VariableSymbol>>
{
    public IReadOnlyCollection<VariableSymbol> Translate( string source )
    {
        var result = new List<VariableSymbol>();

        using var reader = new StringReader( source );
        using var csvReader = new CsvReader( reader, ConstantValue.ReaderConfiguration );

        csvReader.Context.RegisterClassMap<UITypeModelClassMap>();

        var records = csvReader.GetRecords<VariableModel>();

        foreach( var record in records )
        {
            var symbol = new VariableSymbol
            {
                Name             = record.Name,
                ArraySize        = 0,
                BuiltIn          = record.BuiltIn,
                Description      = record.Description,
                BuiltIntoVersion = record.BuiltIntoVersion,
            };

            symbol.DataType = DataTypeUtility.GuessFromSymbolName( symbol.Name );
            symbol.Modifier = ModifierFlag.Const;

            result.Add( symbol );
        }

        return result;
    }
}
