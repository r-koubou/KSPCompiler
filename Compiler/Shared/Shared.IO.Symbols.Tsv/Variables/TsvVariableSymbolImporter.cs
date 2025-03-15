using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Contents;
using KSPCompiler.Shared.IO.Symbols.Tsv.Variables.Translators;

namespace KSPCompiler.Shared.IO.Symbols.Tsv.Variables;

public class TsvVariableSymbolImporter( ITextContentReader reader )
    : TsvSymbolImporter<VariableSymbol, TsvToVariableSymbolTranslator>( reader );
