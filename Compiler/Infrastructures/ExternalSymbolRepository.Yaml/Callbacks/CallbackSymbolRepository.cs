using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Yaml.Callbacks.Models;
using KSPCompiler.ExternalSymbolRepository.Yaml.Callbacks.Translators;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Callbacks;

public class CallbackSymbolRepository( FilePath repositoryPath, bool autoFlush = false )
    : SymbolRepository<CallbackSymbol, CallbackSymbolRootModel, CallBackSymbolModel>(
        repositoryPath,
        new SymbolToSymbolModelTranslator(),
        new SymbolModelToSymbolTranslator(),
        autoFlush
    );
