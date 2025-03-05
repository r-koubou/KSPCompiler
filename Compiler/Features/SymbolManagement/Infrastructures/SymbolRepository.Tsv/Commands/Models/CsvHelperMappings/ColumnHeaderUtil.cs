using CsvHelper;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.ExternalSymbol.Tsv.Commands.Models.CsvHelperMappings;

public static class ColumnHeaderUtil
{
    public static void WriteHeader( CsvWriter csvWriter, int maxArgumentCount = 16 )
    {
        // Header
        csvWriter.WriteField( nameof( CommandModel.Name ) );
        csvWriter.WriteField( nameof( CommandModel.BuiltIn ) );
        csvWriter.WriteField( nameof( CommandModel.BuiltIntoVersion ) );
        csvWriter.WriteField( nameof( CommandModel.Description ) );
        csvWriter.WriteField( nameof( CommandModel.ReturnType ) );

        // Header(Arguments)
        for( var i = 1; i <= maxArgumentCount; i++ )
        {
            csvWriter.WriteField( $"{ConstantValue.ArgumentStartNamePrefix}{i}" );
            csvWriter.WriteField( $"{nameof( CommandArgumentModel.DataType )}{i}" );
            csvWriter.WriteField( $"{nameof( CommandArgumentModel.Description )}{i}" );
        }

        csvWriter.NextRecord();
    }
}
