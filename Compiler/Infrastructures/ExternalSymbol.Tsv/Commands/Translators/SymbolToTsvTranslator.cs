using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using CsvHelper;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.ExternalSymbol.Tsv.Commands.Models;
using KSPCompiler.ExternalSymbol.Tsv.Commands.Models.CsvHelperMappings;

namespace KSPCompiler.ExternalSymbol.Tsv.Commands.Translators;

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

    public IReadOnlyCollection<CommandSymbol> Translate( string source )
    {
        var result = new List<CommandSymbol>();

        using var reader = new StringReader( source );
        using var csvReader = new CsvReader( reader, ConstantValue.ReaderConfiguration );

        csvReader.Context.RegisterClassMap<CommandModelClassMap>();

        var records = csvReader.GetRecords<CommandModel>();

        foreach( var record in records )
        {
            var symbol = new CommandSymbol
            {
                Name             = record.Name,
                BuiltIn          = record.BuiltIn,
                Description      = record.Description,
                BuiltIntoVersion = record.BuiltIntoVersion,
                DataType         = DataTypeUtility.GuessFromTypeString( record.ReturnType )
            };

            foreach( var argument in record.Arguments )
            {
                var uiTypeNames = new List<string>();
                var otherTypeNames = new List<string>();

                DataTypeUtility.GuessFromTypeString( argument.DataType, out var typeFlag, ref uiTypeNames, ref otherTypeNames );

                symbol.AddArgument( new CommandArgumentSymbol( uiTypeNames, otherTypeNames )
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
