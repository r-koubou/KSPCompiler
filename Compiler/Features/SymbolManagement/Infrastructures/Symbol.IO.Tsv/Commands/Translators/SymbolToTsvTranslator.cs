using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using CsvHelper;

using KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Tsv.Commands.Models;
using KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Tsv.Commands.Models.CsvHelperMappings;
using KSPCompiler.Shared;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Tsv.Commands.Translators;

public class SymbolToTsvTranslator : IDataTranslator<IEnumerable<CommandSymbol>, string>
{
    public string Translate( IEnumerable<CommandSymbol> source )
    {
        var models = new List<CommandModel>();

        var symbols = source.ToList();

        foreach( var v in symbols )
        {
            var model = new CommandModel
            {
                Name             = v.Name,
                BuiltIn          = v.BuiltIn,
                BuiltIntoVersion = v.BuiltIntoVersion,
                Description      = v.Description,
                ReturnType       = DataTypeUtility.ToString( v.DataType )
            };

            if( v.Arguments.Count == 0 )
            {
                models.Add( model );

                continue;
            }

            var arguments = new List<CommandArgumentModel>();
            var stringBuilder = new StringBuilder();

            foreach( var x in v.Arguments )
            {
                stringBuilder.Clear();
                DataTypeUtility.ToDataTypeString( stringBuilder, x );

                arguments.Add( new CommandArgumentModel
                    {
                        Name        = x.Name,
                        DataType    = stringBuilder.ToString(),
                        Description = x.Description
                    }
                );
            }

            model.Arguments = arguments;
            models.Add( model );
        }

        using var writer = new StringWriter();
        using var csvWriter = new CsvWriter( writer, ConstantValue.WriterConfiguration );
        var maxArgumentCount = symbols.Max( x => x.Arguments.Count );

        csvWriter.Context.RegisterClassMap<CommandModelClassMap>();

        // Header
        ColumnHeaderUtil.WriteHeader( csvWriter, maxArgumentCount: maxArgumentCount );

        // Body
        csvWriter.WriteRecords( models );
        csvWriter.Flush();

        return writer.ToString();
    }
}
