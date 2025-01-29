using System.Collections.Generic;
using System.IO;
using System.Linq;

using CsvHelper;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbol.Tsv.Callbacks.Models;
using KSPCompiler.ExternalSymbol.Tsv.Callbacks.Models.CsvHelperMappings;

namespace KSPCompiler.ExternalSymbol.Tsv.Callbacks.Translators;

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
        csvWriter.WriteField( nameof( CallbackModel.Name ) );
        csvWriter.WriteField( nameof( CallbackModel.BuiltIn ) );
        csvWriter.WriteField( nameof( CallbackModel.AllowMultipleDeclaration ) );
        csvWriter.WriteField( nameof( CallbackModel.BuiltIntoVersion ) );
        csvWriter.WriteField( nameof( CallbackModel.Description ) );

        // Header(Arguments)
        for( var i = 1; i <= maxArgumentCount; i++ )
        {
            csvWriter.WriteField( $"Argument Name{i}" );
            csvWriter.WriteField( $"{nameof( CallbackArgumentModel.RequiredDeclareOnInit )}{i}" );
            csvWriter.WriteField( $"{nameof( CallbackArgumentModel.Description )}{i}" );
        }

        csvWriter.NextRecord();

        csvWriter.WriteRecords( models );
        csvWriter.Flush();

        return writer.ToString();
    }
}
