using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using CsvHelper;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.ExternalSymbol.Tsv.UITypes.Models;
using KSPCompiler.ExternalSymbol.Tsv.UITypes.Models.CsvHelperMappings;

namespace KSPCompiler.ExternalSymbol.Tsv.UITypes.Translators;

public class SymbolToTsvTranslator : IDataTranslator<IEnumerable<UITypeSymbol>, string>
{
    public string Translate( IEnumerable<UITypeSymbol> source )
    {
        var models = new List<UITypeModel>();

        var symbols = source.ToList();

        foreach( var v in symbols )
        {
            var model = new UITypeModel
            {
                Name             = v.Name,
                BuiltIn          = v.BuiltIn,
                DataType         = DataTypeUtility.ToString( v.DataType ),
                BuiltIntoVersion = v.BuiltIntoVersion,
                Description      = v.Description,
            };

            if( v.InitializerArguments.Count == 0 )
            {
                models.Add( model );

                continue;
            }

            var arguments = new List<UITypeArgumentModel>();
            var stringBuilder = new StringBuilder();

            foreach( var x in v.InitializerArguments )
            {
                stringBuilder.Clear();
                DataTypeUtility.ToDataTypeString( stringBuilder, x );

                arguments.Add( new UITypeArgumentModel
                    {
                        Name        = x.Name,
                        Description = x.Description
                    }
                );
            }

            model.Arguments = arguments;
            models.Add( model );
        }

        using var writer = new StringWriter();
        using var csvWriter = new CsvWriter( writer, ConstantValue.WriterConfiguration );
        var maxArgumentCount = symbols.Max( x => x.InitializerArguments.Count );

        csvWriter.Context.RegisterClassMap<UITypeModelClassMap>();

        // Header
        UITypeColumnHeaderUtil.WriteHeader( csvWriter, maxArgumentCount: maxArgumentCount );

        // Body
        csvWriter.WriteRecords( models );
        csvWriter.Flush();

        return writer.ToString();
    }
}
