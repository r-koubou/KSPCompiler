using System.Globalization;

using CsvHelper.Configuration;

namespace KSPCompiler.ExternalSymbol.Tsv;

public static class ConstantValue
{
    public const string DefaultBuiltIntoVersion = "N/A";
    public const string ArgumentStartNamePrefix = "Argument Name";
    public const string ArgumentStartName = $"{ArgumentStartNamePrefix}1";

    public static readonly CsvConfiguration ReaderConfiguration = new( CultureInfo.InvariantCulture )
    {
        HasHeaderRecord = true,
        MissingFieldFound = null,
        Delimiter       = "\t"
    };

    public static readonly CsvConfiguration WriterConfiguration = new( CultureInfo.InvariantCulture )
    {
        HasHeaderRecord = false,
        Delimiter       = "\t"
    };
}
