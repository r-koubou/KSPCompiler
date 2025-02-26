using System;
using System.Collections.Generic;

using KSPCompiler.UseCases.LanguageServer.Compilation;

namespace KSPCompiler.UseCases.LanguageServer.Symbol;

public sealed class SymbolInformationInputPortDetail(
    ICompilationCacheManager cache,
    ScriptLocation location )
{
    public ICompilationCacheManager Cache { get; } = cache;
    public ScriptLocation Location { get; } = location;
}

public sealed class SymbolInformationInputPort(
    SymbolInformationInputPortDetail handlingInputData
) : InputPort<SymbolInformationInputPortDetail>( handlingInputData );

public sealed class SymbolInformationOutputPort(
    List<DocumentSymbol> symbols,
    bool result,
    Exception? error = null
) : OutputPort<List<DocumentSymbol>>( symbols, result, error );

public interface ISymbolInformationUseCase
    : IUseCase<SymbolInformationInputPort, SymbolInformationOutputPort>;
