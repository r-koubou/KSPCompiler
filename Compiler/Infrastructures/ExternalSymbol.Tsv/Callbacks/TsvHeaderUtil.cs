using CsvHelper;

using KSPCompiler.ExternalSymbol.Tsv.Callbacks.Models;

namespace KSPCompiler.ExternalSymbol.Tsv.Callbacks;

public static class TsvHeaderUtil
{
    public static void WriteHeader( CsvWriter csvWriter, int maxArgumentCount = 16 )
    {
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
    }
}
