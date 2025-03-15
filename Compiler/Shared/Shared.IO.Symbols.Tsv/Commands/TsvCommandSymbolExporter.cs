using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Contents;
using KSPCompiler.Shared.IO.Symbols.Tsv.Commands.Models.CsvHelperMappings;
using KSPCompiler.Shared.IO.Symbols.Tsv.Commands.Translators;

namespace KSPCompiler.Shared.IO.Symbols.Tsv.Commands;

public class TsvCommandSymbolExporter( ITextContentWriter contentWriter )
    : TsvSymbolExporter<
        CommandSymbol,
        CommandSymbolToTsvTranslator,
        CommandSymbolHeaderRecordWriter
    >( contentWriter );
