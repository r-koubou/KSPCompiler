using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Contents;
using KSPCompiler.Shared.IO.Symbols.Tsv.Variables.Models.CsvHelperMappings;
using KSPCompiler.Shared.IO.Symbols.Tsv.Variables.Translators;

namespace KSPCompiler.Shared.IO.Symbols.Tsv.Variables;

public class TsvVariableSymbolExporter( ITextContentWriter contentWriter )
    : TsvSymbolExporter<
        VariableSymbol,
        VariableSymbolToTsvTranslator,
        VariableHeaderRecordWriter
    >( contentWriter );
