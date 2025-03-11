using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Contents;
using KSPCompiler.Shared.IO.Symbols.Yaml.UITypes.Models;
using KSPCompiler.Shared.IO.Symbols.Yaml.UITypes.Translators;

namespace KSPCompiler.Shared.IO.Symbols.Yaml.UITypes;

public class YamlUITypeSymbolImporter( ITextContentReader writer ) :
    YamlSymbolImporter<
        UITypeSymbol,
        UITypeSymbolRootModel,
        UITypeSymbolModel,
        SymbolModelToSymbolTranslator
    >( writer );
