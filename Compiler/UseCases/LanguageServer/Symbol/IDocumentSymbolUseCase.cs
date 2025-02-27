using System;
using System.Collections.Generic;

using KSPCompiler.UseCases.LanguageServer.Compilation;

namespace KSPCompiler.UseCases.LanguageServer.Symbol;

public sealed class DocumentSymbolInputPortDetail(
    ICompilationCacheManager cache,
    ScriptLocation location )
{
    public ICompilationCacheManager Cache { get; } = cache;
    public ScriptLocation Location { get; } = location;
}

public sealed class DocumentSymbolInputPort(
    DocumentSymbolInputPortDetail handlingInputData
) : InputPort<DocumentSymbolInputPortDetail>( handlingInputData );

public sealed class DocumentSymbolOutputPort(
    List<DocumentSymbol> symbols,
    bool result,
    Exception? error = null
) : OutputPort<List<DocumentSymbol>>( symbols, result, error );

public interface IDocumentSymbolUseCase
    : IUseCase<DocumentSymbolInputPort, DocumentSymbolOutputPort>;
