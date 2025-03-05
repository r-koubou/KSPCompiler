using KSPCompiler.Features.SymbolManagement.Infrastructures.SymbolRepository.Yaml.UITypes.Models;
using KSPCompiler.Features.SymbolManagement.Infrastructures.SymbolRepository.Yaml.UITypes.Translators;
using KSPCompiler.Shared.Contents;
using KSPCompiler.Shared.Domain.Symbols;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.SymbolRepository.Yaml.UITypes;

public class UITypeSymbolExporter( ITextContentWriter writer ) :
    SymbolExporter<
        UITypeSymbol,
        UITypeSymbolRootModel,
        UITypeSymbolModel,
        SymbolToSymbolModelTranslator
    >( writer );
