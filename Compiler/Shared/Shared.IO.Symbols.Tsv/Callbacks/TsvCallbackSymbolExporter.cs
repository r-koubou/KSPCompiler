using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Contents;
using KSPCompiler.Shared.IO.Symbols.Tsv.Callbacks.Models.CsvHelperMappings;
using KSPCompiler.Shared.IO.Symbols.Tsv.Callbacks.Translators;

namespace KSPCompiler.Shared.IO.Symbols.Tsv.Callbacks;

public sealed class TsvCallbackSymbolExporter( ITextContentWriter contentWriter )
    : TsvSymbolExporter<
        CallbackSymbol,
        CallbackSymbolToTsvTranslator,
        CallbackHeaderRecordWriter
    >( contentWriter );
