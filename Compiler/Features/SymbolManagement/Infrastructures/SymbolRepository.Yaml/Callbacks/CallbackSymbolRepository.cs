using KSPCompiler.Shared.Domain.Symbols;
using KSPCompiler.Shared.Path;
using KSPCompiler.SymbolManagement.Repository.Yaml.Callbacks.Models;
using KSPCompiler.SymbolManagement.Repository.Yaml.Callbacks.Translators;

namespace KSPCompiler.SymbolManagement.Repository.Yaml.Callbacks;

public class CallbackSymbolRepository( FilePath repositoryPath, bool autoFlush = false )
    : SymbolRepository<CallbackSymbol, CallbackSymbolRootModel, CallBackSymbolModel>(
        repositoryPath,
        new SymbolToSymbolModelTranslator(),
        new SymbolModelToSymbolTranslator(),
        autoFlush
    );
