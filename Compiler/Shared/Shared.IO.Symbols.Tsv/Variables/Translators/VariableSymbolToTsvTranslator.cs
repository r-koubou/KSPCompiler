using System.Collections.Generic;
using System.IO;
using System.Linq;

using CsvHelper;

using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Symbols.Tsv.Variables.Models;
using KSPCompiler.Shared.IO.Symbols.Tsv.Variables.Models.CsvHelperMappings;

namespace KSPCompiler.Shared.IO.Symbols.Tsv.Variables.Translators;

public class VariableSymbolToTsvTranslator : IDataTranslator<IEnumerable<VariableSymbol>, string>
{
    public string Translate( IEnumerable<VariableSymbol> source )
    {
        var models = new List<VariableModel>();

        var symbols = source.ToList();

        foreach( var v in symbols )
        {
            var model = new VariableModel
            {
                Name             = v.Name,
                BuiltIn          = v.BuiltIn,
                Description      = v.Description,
                BuiltIntoVersion = v.BuiltIntoVersion
            };

            models.Add( model );
        }

        using var writer = new StringWriter();
        using var csvWriter = new CsvWriter( writer, ConstantValue.WriterConfiguration );

        csvWriter.Context.RegisterClassMap<VariableModelClassMap>();

        // Header
        var headerWriter = new VariableHeaderRecordWriter();
        headerWriter.WriteHeaderRecord( csvWriter );

        // Body
        csvWriter.WriteRecords( models );
        csvWriter.Flush();

        return writer.ToString();
    }
}
