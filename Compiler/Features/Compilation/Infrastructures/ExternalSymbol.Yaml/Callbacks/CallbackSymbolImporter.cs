using KSPCompiler.Features.Compilation.Infrastructures.ExternalSymbol.Yaml.Callbacks.Models;
using KSPCompiler.Features.Compilation.Infrastructures.ExternalSymbol.Yaml.Callbacks.Translators;
using KSPCompiler.Shared.Contents;
using KSPCompiler.Shared.Domain.Symbols;

namespace KSPCompiler.Features.Compilation.Infrastructures.ExternalSymbol.Yaml.Callbacks;

public class CallbackSymbolImporter( ITextContentReader writer ) :
    SymbolImporter<
        CallbackSymbol,
        CallbackSymbolRootModel,
        CallBackSymbolModel,
        SymbolModelToSymbolTranslator
    >( writer );
