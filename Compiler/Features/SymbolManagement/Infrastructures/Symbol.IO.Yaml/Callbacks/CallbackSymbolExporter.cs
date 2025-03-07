using KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Repository.Yaml.Callbacks.Models;
using KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Repository.Yaml.Callbacks.Translators;
using KSPCompiler.Shared.Contents;
using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Repository.Yaml.Callbacks;

public class CallbackSymbolExporter( ITextContentWriter writer ) :
    SymbolExporter<
        CallbackSymbol,
        CallbackSymbolRootModel,
        CallBackSymbolModel,
        SymbolToSymbolModelTranslator
    >( writer );
