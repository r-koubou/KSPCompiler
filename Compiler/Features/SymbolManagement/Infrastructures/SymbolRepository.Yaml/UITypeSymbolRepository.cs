using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.EventEmitting;
using KSPCompiler.Shared.IO.Abstractions.Symbol;

namespace KSPCompiler.SymbolManagement.Repository.Yaml;

public class UITypeSymbolRepository(
    ISymbolImporter<UITypeSymbol>? repositoryImporter = null,
    ISymbolExporter<UITypeSymbol>? repositoryExporter = null,
    IEventEmitter? eventEmitter = null,
    bool autoFlush = true )
    : SymbolRepository<UITypeSymbol>(
        repositoryImporter: repositoryImporter,
        repositoryExporter: repositoryExporter,
        eventEmitter: eventEmitter,
        autoFlush: autoFlush
    );
