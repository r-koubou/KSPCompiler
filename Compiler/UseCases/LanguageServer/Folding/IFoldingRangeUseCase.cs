using System;
using System.Collections.Generic;

using KSPCompiler.UseCases.LanguageServer.Compilation;

namespace KSPCompiler.UseCases.LanguageServer.Folding;

public sealed class FoldingRangeInputPortDetail(
    ICompilationCacheManager cache,
    ScriptLocation location
)
{
    public ICompilationCacheManager Cache { get; } = cache;
    public ScriptLocation Location { get; } = location;
}

public sealed class FoldingRangeInputPort(
    FoldingRangeInputPortDetail input
) : InputPort<FoldingRangeInputPortDetail>( input );

public sealed class FoldingRangeOutputPort(
    List<FoldingItem> ranges,
    bool result,
    Exception? error = null
) : OutputPort<List<FoldingItem>>( ranges, result, error );

public interface IFoldingRangeUseCase
    : IUseCase<FoldingRangeInputPort, FoldingRangeOutputPort>;
