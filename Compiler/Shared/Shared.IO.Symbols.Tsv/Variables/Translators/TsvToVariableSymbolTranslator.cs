using System;
using System.Collections.Generic;
using System.IO;

using CsvHelper;

using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;
using KSPCompiler.Shared.IO.Symbols.Tsv.Variables.Models;
using KSPCompiler.Shared.IO.Symbols.Tsv.Variables.Models.CsvHelperMappings;

namespace KSPCompiler.Shared.IO.Symbols.Tsv.Variables.Translators;

public sealed class TsvToVariableSymbolTranslator : IDataTranslator<string, IReadOnlyCollection<VariableSymbol>>
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
                Id               = Guid.Parse( record.Id ),
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
