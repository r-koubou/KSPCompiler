using System.Collections.Generic;
using System.IO;

using CsvHelper;

using KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Tsv.Variables.Models;
using KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Tsv.Variables.Models.CsvHelperMappings;
using KSPCompiler.Shared;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Tsv.Variables.Translators;

public class TsvToSymbolTranslator : IDataTranslator<string, IReadOnlyCollection<VariableSymbol>>
{
    public IReadOnlyCollection<VariableSymbol> Translate( string source )
    {
        var result = new List<VariableSymbol>();

        using var reader = new StringReader( source );
        using var csvReader = new CsvReader( reader, ConstantValue.ReaderConfiguration );

        csvReader.Context.RegisterClassMap<VariableModelClassMap>();

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
