using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Contents;
using KSPCompiler.Shared.IO.Symbols.Tsv.UITypes.Models.CsvHelperMappings;
using KSPCompiler.Shared.IO.Symbols.Tsv.UITypes.Translators;

namespace KSPCompiler.Shared.IO.Symbols.Tsv.UITypes;

public sealed class TsvUITypeSymbolExporter( ITextContentWriter contentWriter )
    : TsvSymbolExporter<
        UITypeSymbol,
        UITypeSymbolToTsvTranslator,
        UITypeHeaderRecordWriter
    >( contentWriter );

