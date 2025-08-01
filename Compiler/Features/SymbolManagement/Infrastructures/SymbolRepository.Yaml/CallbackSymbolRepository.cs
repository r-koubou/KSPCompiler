using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.EventEmitting;
using KSPCompiler.Shared.IO.Abstractions.Symbol;

namespace KSPCompiler.SymbolManagement.Repository.Yaml;

public class CallbackSymbolRepository(
    ISymbolImporter<CallbackSymbol>? repositoryImporter = null,
    ISymbolExporter<CallbackSymbol>? repositoryExporter = null,
    IEventEmitter? eventEmitter = null,
    bool autoFlush = true )
    : SymbolRepository<CallbackSymbol>(
        repositoryImporter: repositoryImporter,
        repositoryExporter: repositoryExporter,
        eventEmitter: eventEmitter,
        autoFlush: autoFlush
    );
