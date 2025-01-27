using KSPCompiler.Commons.Contents;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbol.Yaml.Callbacks.Models;
using KSPCompiler.ExternalSymbol.Yaml.Callbacks.Translators;

namespace KSPCompiler.ExternalSymbol.Yaml.Callbacks;

public class CallbackSymbolImporter( ITextContentReader writer ) :
    SymbolImporter<
        CallbackSymbol,
        CallbackSymbolRootModel,
        CallBackSymbolModel,
        SymbolModelToSymbolTranslator
    >( writer );
