using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Contents;
using KSPCompiler.Shared.IO.Symbols.Yaml.Callbacks.Models;
using KSPCompiler.Shared.IO.Symbols.Yaml.Callbacks.Translators;

namespace KSPCompiler.Shared.IO.Symbols.Yaml.Callbacks;

public class CallbackSymbolExporter( ITextContentWriter writer ) :
    SymbolExporter<
        CallbackSymbol,
        CallbackSymbolRootModel,
        CallBackSymbolModel,
        SymbolToSymbolModelTranslator
    >( writer );
