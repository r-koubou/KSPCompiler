using System;
using System.Collections.Generic;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Compilation;
using KSPCompiler.Shared.UseCase;

namespace KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Folding;

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
