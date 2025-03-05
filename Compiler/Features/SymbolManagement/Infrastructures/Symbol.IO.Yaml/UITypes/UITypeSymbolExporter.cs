using KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Repository.Yaml.UITypes.Models;
using KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Repository.Yaml.UITypes.Translators;
using KSPCompiler.Shared.Contents;
using KSPCompiler.Shared.Domain.Symbols;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Repository.Yaml.UITypes;

public class UITypeSymbolExporter( ITextContentWriter writer ) :
    SymbolExporter<
        UITypeSymbol,
        UITypeSymbolRootModel,
        UITypeSymbolModel,
        SymbolToSymbolModelTranslator
    >( writer );
