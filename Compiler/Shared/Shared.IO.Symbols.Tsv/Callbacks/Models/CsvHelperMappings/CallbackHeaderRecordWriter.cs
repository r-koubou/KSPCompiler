using CsvHelper;

namespace KSPCompiler.Shared.IO.Symbols.Tsv.Callbacks.Models.CsvHelperMappings;

public sealed class CallbackHeaderRecordWriter : ITsvHeaderRecordWriter
{
    public void WriteHeaderRecord( CsvWriter csvWriter, int maxArgumentCount = 16 )
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
            csvWriter.WriteField( $"{ConstantValue.ArgumentStartNamePrefix}{i}" );
            csvWriter.WriteField( $"{nameof( CallbackArgumentModel.RequiredDeclareOnInit )}{i}" );
            csvWriter.WriteField( $"{nameof( CallbackArgumentModel.Description )}{i}" );
        }

        csvWriter.NextRecord();
    }
}
