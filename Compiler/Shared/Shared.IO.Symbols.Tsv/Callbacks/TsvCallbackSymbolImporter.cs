using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Contents;
using KSPCompiler.Shared.IO.Symbols.Tsv.Callbacks.Translators;

namespace KSPCompiler.Shared.IO.Symbols.Tsv.Callbacks;

public class TsvCallbackSymbolImporter( ITextContentReader reader )
    : TsvSymbolImporter<CallbackSymbol, TsvToCallbackSymbolTranslator>( reader );
