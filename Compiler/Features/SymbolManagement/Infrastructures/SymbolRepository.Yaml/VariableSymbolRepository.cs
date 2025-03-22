using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Symbol;

namespace KSPCompiler.SymbolManagement.Repository.Yaml;

public class VariableSymbolRepository(
    ISymbolImporter<VariableSymbol>? repositoryImporter = null,
    ISymbolExporter<VariableSymbol>? repositoryExporter = null,
    bool autoFlush = true )
    : SymbolRepository<VariableSymbol>(
        repositoryImporter,
        repositoryExporter,
        autoFlush
    );
