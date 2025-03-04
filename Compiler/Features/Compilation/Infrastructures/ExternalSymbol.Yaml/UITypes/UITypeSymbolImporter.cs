using KSPCompiler.Features.Compilation.Infrastructures.ExternalSymbol.Yaml.UITypes.Models;
using KSPCompiler.Features.Compilation.Infrastructures.ExternalSymbol.Yaml.UITypes.Translators;
using KSPCompiler.Shared.Contents;
using KSPCompiler.Shared.Domain.Symbols;

namespace KSPCompiler.Features.Compilation.Infrastructures.ExternalSymbol.Yaml.UITypes;

public class UITypeSymbolImporter( ITextContentReader writer ) :
    SymbolImporter<
        UITypeSymbol,
        UITypeSymbolRootModel,
        UITypeSymbolModel,
        SymbolModelToSymbolTranslator
    >( writer );
