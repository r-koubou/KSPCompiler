using KSPCompiler.Commons.Contents;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbol.Yaml.Callbacks.Models;
using KSPCompiler.ExternalSymbol.Yaml.Callbacks.Translators;

namespace KSPCompiler.ExternalSymbol.Yaml.Callbacks;

public class CallbackSymbolExporter( ITextContentWriter writer ) :
    SymbolExporter<
        CallbackSymbol,
        CallbackSymbolRootModel,
        CallBackSymbolModel,
        SymbolToSymbolModelTranslator
    >( writer );
