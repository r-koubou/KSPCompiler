using System;
using System.Collections.Generic;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Compilation;
using KSPCompiler.Shared.Text;
using KSPCompiler.Shared.UseCase;

namespace KSPCompiler.Features.LanguageServer.UseCase.Abstractions.FindReferences;

public sealed class FindReferenceInputPortDetail(
    ICompilationCacheManager cache,
    ScriptLocation location,
    Position position
)
{
    public ICompilationCacheManager Cache { get; } = cache;
    public ScriptLocation Location { get; } = location;
    public Position Position { get; } = position;
}

public sealed class FindReferenceInputPort(
    FindReferenceInputPortDetail inputInput
) : InputPort<FindReferenceInputPortDetail>( inputInput );

public sealed class FindReferenceOutputPort(
    List<ReferenceItem> references,
    bool result,
    Exception? error = null
) : OutputPort<List<ReferenceItem>>( references, result, error );

public interface IFindReferenceUseCase
    : IUseCase<FindReferenceInputPort, FindReferenceOutputPort>;
