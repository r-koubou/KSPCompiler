using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Symbol;

namespace KSPCompiler.SymbolManagement.Repository.Yaml;

public class CommandSymbolRepository(
    ISymbolImporter<CommandSymbol>? repositoryReader = null,
    ISymbolExporter<CommandSymbol>? repositoryWriter = null,
    bool autoFlush = true )
    : SymbolRepository<CommandSymbol>(
        repositoryReader,
        repositoryWriter,
        autoFlush
    );
