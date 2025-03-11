using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Symbol;

namespace KSPCompiler.SymbolManagement.Repository.Yaml;

public class CallbackSymbolRepository(
    ISymbolImporter<CallbackSymbol>? repositoryReader = null,
    ISymbolExporter<CallbackSymbol>? repositoryWriter = null,
    bool autoFlush = true )
    : SymbolRepository<CallbackSymbol>(
        repositoryReader,
        repositoryWriter,
        autoFlush
    );
