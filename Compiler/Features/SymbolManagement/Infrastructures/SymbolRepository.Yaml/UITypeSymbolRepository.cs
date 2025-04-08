using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Symbol;

namespace KSPCompiler.SymbolManagement.Repository.Yaml;

public class UITypeSymbolRepository(
    ISymbolImporter<UITypeSymbol>? repositoryImporter = null,
    ISymbolExporter<UITypeSymbol>? repositoryExporter = null,
    bool autoFlush = true )
    : SymbolRepository<UITypeSymbol>(
        repositoryImporter,
        repositoryExporter,
        autoFlush
    );
