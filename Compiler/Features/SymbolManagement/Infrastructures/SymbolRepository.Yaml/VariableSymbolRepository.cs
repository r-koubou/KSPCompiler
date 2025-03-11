using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Symbol;

namespace KSPCompiler.SymbolManagement.Repository.Yaml;

public class VariableSymbolRepository(
    ISymbolImporter<VariableSymbol>? repositoryReader = null,
    ISymbolExporter<VariableSymbol>? repositoryWriter = null,
    bool autoFlush = true )
    : SymbolRepository<VariableSymbol>(
        repositoryReader,
        repositoryWriter,
        autoFlush
    );
