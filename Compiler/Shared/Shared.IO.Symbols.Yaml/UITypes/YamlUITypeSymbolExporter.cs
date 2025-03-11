using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Contents;
using KSPCompiler.Shared.IO.Symbols.Yaml.UITypes.Models;
using KSPCompiler.Shared.IO.Symbols.Yaml.UITypes.Translators;

namespace KSPCompiler.Shared.IO.Symbols.Yaml.UITypes;

public class YamlUITypeSymbolExporter( ITextContentWriter writer ) :
    YamlSymbolExporter<
        UITypeSymbol,
        UITypeSymbolRootModel,
        UITypeSymbolModel,
        SymbolToSymbolModelTranslator
    >( writer );
