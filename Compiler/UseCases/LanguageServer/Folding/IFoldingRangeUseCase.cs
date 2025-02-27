using System;
using System.Collections.Generic;

using KSPCompiler.UseCases.LanguageServer.Compilation;

namespace KSPCompiler.UseCases.LanguageServer.Folding;

public sealed class FoldingInputPortDetail(
    ICompilationCacheManager cache,
    ScriptLocation location
)
{
    public ICompilationCacheManager Cache { get; } = cache;
    public ScriptLocation Location { get; } = location;
}

public sealed class FoldingInputPort(
    FoldingInputPortDetail handlingInputData
) : InputPort<FoldingInputPortDetail>( handlingInputData );

public sealed class FoldingOutputPort(
    List<FoldingItem> ranges,
    bool result,
    Exception? error = null
) : OutputPort<List<FoldingItem>>( ranges, result, error );

public interface IFoldingRangeUseCase
    : IUseCase<FoldingInputPort, FoldingOutputPort>;
