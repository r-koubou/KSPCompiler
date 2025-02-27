using System;
using System.Collections.Generic;

using KSPCompiler.Commons.Text;
using KSPCompiler.UseCases.LanguageServer.Compilation;

namespace KSPCompiler.UseCases.LanguageServer.FindReferences;

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
