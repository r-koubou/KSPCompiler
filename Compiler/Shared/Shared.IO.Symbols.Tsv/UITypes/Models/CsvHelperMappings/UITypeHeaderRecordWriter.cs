using CsvHelper;

namespace KSPCompiler.Shared.IO.Symbols.Tsv.UITypes.Models.CsvHelperMappings;

public sealed class UITypeHeaderRecordWriter : ITsvHeaderRecordWriter
{
    public void WriteHeaderRecord( CsvWriter csvWriter, int maxArgumentCount = 16 )
    {
        // Header
        csvWriter.WriteField( nameof( UITypeModel.Id ) );
        csvWriter.WriteField( nameof( UITypeModel.Name ) );
        csvWriter.WriteField( nameof( UITypeModel.BuiltIn ) );
        csvWriter.WriteField( nameof( UITypeModel.DataType ) );
        csvWriter.WriteField( nameof( UITypeModel.Description ) );
        csvWriter.WriteField( nameof( UITypeModel.BuiltIntoVersion ) );
        csvWriter.WriteField( nameof( UITypeModel.InitializerRequired ) );

        // Header(Arguments)
        for( var i = 1; i <= maxArgumentCount; i++ )
        {
            csvWriter.WriteField( $"{ConstantValue.ArgumentStartNamePrefix}{i}" );
            csvWriter.WriteField( $"{nameof( UITypeArgumentModel.Description )}{i}" );
        }

        csvWriter.NextRecord();
    }
}
