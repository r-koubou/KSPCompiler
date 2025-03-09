using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Symbol;

namespace KSPCompiler.SymbolManagement.Repository.Yaml;

public class UITypeSymbolRepository(
    ISymbolImporter<UITypeSymbol>? repositoryReader = null,
    ISymbolExporter<UITypeSymbol>? repositoryWriter = null,
    bool autoFlush = true )
    : SymbolRepository<UITypeSymbol>(
        repositoryReader,
        repositoryWriter,
        autoFlush
    );
