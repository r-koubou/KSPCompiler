using System;
using System.Collections.Generic;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Compilation;
using KSPCompiler.Shared.UseCase;

namespace KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Symbol;

public sealed class DocumentSymbolInputPortDetail(
    ICompilationCacheManager cache,
    ScriptLocation location )
{
    public ICompilationCacheManager Cache { get; } = cache;
    public ScriptLocation Location { get; } = location;
}

public sealed class DocumentSymbolInputPort(
    DocumentSymbolInputPortDetail input
) : InputPort<DocumentSymbolInputPortDetail>( input );

public sealed class DocumentSymbolOutputPort(
    List<DocumentSymbol> symbols,
    bool result,
    Exception? error = null
) : OutputPort<List<DocumentSymbol>>( symbols, result, error );

public interface IDocumentSymbolUseCase
    : IUseCase<DocumentSymbolInputPort, DocumentSymbolOutputPort>;
