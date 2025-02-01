using System.Collections.Generic;
using System.IO;
using System.Linq;

using CsvHelper;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbol.Tsv.Variables.Models;
using KSPCompiler.ExternalSymbol.Tsv.Variables.Models.CsvHelperMappings;

namespace KSPCompiler.ExternalSymbol.Tsv.Variables.Translators;

public class SymbolToTsvTranslator : IDataTranslator<IEnumerable<VariableSymbol>, string>
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
        VariableColumnHeaderUtil.WriteHeader( csvWriter );

        // Body
        csvWriter.WriteRecords( models );
        csvWriter.Flush();

        return writer.ToString();
    }
}
