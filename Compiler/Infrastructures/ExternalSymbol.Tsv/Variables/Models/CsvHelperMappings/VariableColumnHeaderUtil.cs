using CsvHelper;

namespace KSPCompiler.ExternalSymbol.Tsv.Variables.Models.CsvHelperMappings;

public static class VariableColumnHeaderUtil
{
    public static void WriteHeader( CsvWriter csvWriter )
    {
        // Header
        csvWriter.WriteField( nameof( VariableModel.Name ) );
        csvWriter.WriteField( nameof( VariableModel.BuiltIn ) );
        csvWriter.WriteField( nameof( VariableModel.Description ) );
        csvWriter.WriteField( nameof( VariableModel.BuiltIntoVersion ) );

        csvWriter.NextRecord();
    }
}
