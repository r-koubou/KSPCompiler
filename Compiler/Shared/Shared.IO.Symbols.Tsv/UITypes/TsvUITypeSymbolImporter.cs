using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Contents;
using KSPCompiler.Shared.IO.Symbols.Tsv.UITypes.Translators;

namespace KSPCompiler.Shared.IO.Symbols.Tsv.UITypes;

public class TsvUITypeSymbolImporter( ITextContentReader reader )
    : TsvSymbolImporter<UITypeSymbol, TsvToUITypeSymbolTranslator>( reader );
