using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Symbol;

namespace KSPCompiler.SymbolManagement.Repository.Yaml;

public class CommandSymbolRepository(
    ISymbolImporter<CommandSymbol>? repositoryImporter = null,
    ISymbolExporter<CommandSymbol>? repositoryExporter = null,
    bool autoFlush = true )
    : SymbolRepository<CommandSymbol>(
        repositoryImporter,
        repositoryExporter,
        autoFlush
    );
