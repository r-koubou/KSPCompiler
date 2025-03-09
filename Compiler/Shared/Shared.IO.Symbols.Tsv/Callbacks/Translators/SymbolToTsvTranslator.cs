using System.Collections.Generic;
using System.IO;
using System.Linq;

using CsvHelper;

using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Symbols.Tsv.Callbacks.Models;
using KSPCompiler.Shared.IO.Symbols.Tsv.Callbacks.Models.CsvHelperMappings;

namespace KSPCompiler.Shared.IO.Symbols.Tsv.Callbacks.Translators;

internal class SymbolToTsvTranslator : IDataTranslator<IEnumerable<CallbackSymbol>, string>
{
    public string Translate( IEnumerable<CallbackSymbol> source )
    {
        var models = new List<CallbackModel>();

        var symbols = source.ToList();

        foreach( var v in symbols )
        {
            var model = new CallbackModel
            {
                Name                     = v.Name,
                BuiltIn                  = v.BuiltIn,
                AllowMultipleDeclaration = v.AllowMultipleDeclaration,
                BuiltIntoVersion         = v.BuiltIntoVersion,
                Description              = v.Description
            };

            if( v.Arguments.Count == 0 )
            {
                models.Add( model );

                continue;
            }

            var arguments = new List<CallbackArgumentModel>();

            foreach( var x in v.Arguments )
            {
                arguments.Add( new CallbackArgumentModel
                    {
                        Name                  = x.Name,
                        RequiredDeclareOnInit = x.RequiredDeclareOnInit,
                        Description           = x.Description
                    }
                );
            }

            model.Arguments = arguments;
            models.Add( model );
        }

        using var writer = new StringWriter();
        using var csvWriter = new CsvWriter( writer, ConstantValue.WriterConfiguration );
        var maxArgumentCount = symbols.Max( x => x.Arguments.Count );

        csvWriter.Context.RegisterClassMap<CallbackModelClassMap>();

        // Header
        ColumnHeaderUtil.WriteHeader( csvWriter, maxArgumentCount: maxArgumentCount );

        // Body
        csvWriter.WriteRecords( models );
        csvWriter.Flush();

        return writer.ToString();
    }
}
