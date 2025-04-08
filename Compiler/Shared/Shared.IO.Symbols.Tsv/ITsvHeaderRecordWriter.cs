using CsvHelper;

namespace KSPCompiler.Shared.IO.Symbols.Tsv;

public interface ITsvHeaderRecordWriter
{
    void WriteHeaderRecord( CsvWriter csvWriter, int maxArgumentCount = 16 );
}
