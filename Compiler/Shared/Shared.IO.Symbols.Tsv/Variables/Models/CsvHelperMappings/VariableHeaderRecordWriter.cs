using CsvHelper;

namespace KSPCompiler.Shared.IO.Symbols.Tsv.Variables.Models.CsvHelperMappings;

public sealed class VariableHeaderRecordWriter : ITsvHeaderRecordWriter
{
    public void WriteHeaderRecord( CsvWriter csvWriter, int maxArgumentCount = 16 )
    {
        // Header
        csvWriter.WriteField( nameof( VariableModel.Name ) );
        csvWriter.WriteField( nameof( VariableModel.BuiltIn ) );
        csvWriter.WriteField( nameof( VariableModel.Description ) );
        csvWriter.WriteField( nameof( VariableModel.BuiltIntoVersion ) );

        csvWriter.NextRecord();
    }
}
