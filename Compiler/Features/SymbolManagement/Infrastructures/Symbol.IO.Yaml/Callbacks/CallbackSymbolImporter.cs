using KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Repository.Yaml.Callbacks.Models;
using KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Repository.Yaml.Callbacks.Translators;
using KSPCompiler.Shared.Contents;
using KSPCompiler.Shared.Domain.Symbols;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Repository.Yaml.Callbacks;

public class CallbackSymbolImporter( ITextContentReader writer ) :
    SymbolImporter<
        CallbackSymbol,
        CallbackSymbolRootModel,
        CallBackSymbolModel,
        SymbolModelToSymbolTranslator
    >( writer );
