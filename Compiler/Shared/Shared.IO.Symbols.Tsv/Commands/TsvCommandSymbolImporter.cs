using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Contents;
using KSPCompiler.Shared.IO.Symbols.Tsv.Commands.Translators;

namespace KSPCompiler.Shared.IO.Symbols.Tsv.Commands;

public class TsvCommandSymbolImporter( ITextContentReader reader )
    : TsvSymbolImporter<CommandSymbol, TsvToCommandSymbolTranslator>( reader );
