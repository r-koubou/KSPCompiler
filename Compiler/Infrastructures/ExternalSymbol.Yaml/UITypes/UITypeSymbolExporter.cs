using KSPCompiler.Commons.Contents;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbol.Yaml.UITypes.Models;
using KSPCompiler.ExternalSymbol.Yaml.UITypes.Translators;

namespace KSPCompiler.ExternalSymbol.Yaml.UITypes;

public class UITypeSymbolExporter( ITextContentWriter writer ) :
    SymbolExporter<
        UITypeSymbol,
        UITypeSymbolRootModel,
        UITypeSymbolModel,
        SymbolToSymbolModelTranslator
    >( writer );
