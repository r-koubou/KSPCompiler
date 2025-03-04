using KSPCompiler.Features.Compilation.Infrastructures.ExternalSymbolRepository.Yaml.Callbacks.Models;
using KSPCompiler.Features.Compilation.Infrastructures.ExternalSymbolRepository.Yaml.Callbacks.Translators;
using KSPCompiler.Shared.Domain.Symbols;
using KSPCompiler.Shared.Path;

namespace KSPCompiler.Features.Compilation.Infrastructures.ExternalSymbolRepository.Yaml.Callbacks;

public class CallbackSymbolRepository( FilePath repositoryPath, bool autoFlush = false )
    : SymbolRepository<CallbackSymbol, CallbackSymbolRootModel, CallBackSymbolModel>(
        repositoryPath,
        new SymbolModelToSymbolTranslator(),
        autoFlush
    );
