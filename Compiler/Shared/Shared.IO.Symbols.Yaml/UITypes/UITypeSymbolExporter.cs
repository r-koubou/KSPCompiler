using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Contents;
using KSPCompiler.Shared.IO.Symbols.Yaml.UITypes.Models;
using KSPCompiler.Shared.IO.Symbols.Yaml.UITypes.Translators;

namespace KSPCompiler.Shared.IO.Symbols.Yaml.UITypes;

public class UITypeSymbolExporter( ITextContentWriter writer ) :
    SymbolExporter<
        UITypeSymbol,
        UITypeSymbolRootModel,
        UITypeSymbolModel,
        SymbolToSymbolModelTranslator
    >( writer );
