using KSPCompiler.Features.SymbolManagement.Infrastructures.SymbolRepository.Yaml.Callbacks.Models;
using KSPCompiler.Features.SymbolManagement.Infrastructures.SymbolRepository.Yaml.Callbacks.Translators;
using KSPCompiler.Shared.Contents;
using KSPCompiler.Shared.Domain.Symbols;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.SymbolRepository.Yaml.Callbacks;

public class CallbackSymbolExporter( ITextContentWriter writer ) :
    SymbolExporter<
        CallbackSymbol,
        CallbackSymbolRootModel,
        CallBackSymbolModel,
        SymbolToSymbolModelTranslator
    >( writer );
