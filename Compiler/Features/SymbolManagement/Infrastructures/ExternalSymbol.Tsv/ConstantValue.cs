using System.Globalization;

using CsvHelper.Configuration;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.ExternalSymbol.Tsv;

public static class ConstantValue
{
    private const string Delimiter = "\t";

    public const string DefaultBuiltIntoVersion = "N/A";
    public const string ArgumentStartNamePrefix = "Argument Name";
    public const string ArgumentStartName = $"{ArgumentStartNamePrefix}1";

    public static readonly CsvConfiguration ReaderConfiguration = new( CultureInfo.InvariantCulture )
    {
        HasHeaderRecord   = true,
        MissingFieldFound = null,
        Delimiter         = Delimiter
    };

    public static readonly CsvConfiguration WriterConfiguration = new( CultureInfo.InvariantCulture )
    {
        HasHeaderRecord = false,
        Delimiter       = Delimiter
    };
}
