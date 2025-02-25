using System;
using System.Collections.Generic;

using KSPCompiler.Commons.Text;
using KSPCompiler.UseCases.LanguageServer.Compilation;

namespace KSPCompiler.UseCases.LanguageServer.Definition;

public sealed class DefinitionInputPortDetail(
    ICompilationCacheManager cache,
    ScriptLocation location,
    Position position
)
{
    public ICompilationCacheManager Cache { get; } = cache;
    public ScriptLocation Location { get; } = location;
    public Position Position { get; } = position;
}

public sealed class DefinitionInputPort(
    DefinitionInputPortDetail handlingInputData
) : InputPort<DefinitionInputPortDetail>( handlingInputData );

public sealed class DefinitionOutputPort(
    List<LocationLink> links,
    bool result,
    Exception? error = null
) : OutputPort<List<LocationLink>>( links, result, error );

public interface IDefinitionHandlingInteractor
    : IUseCase<DefinitionInputPort, DefinitionOutputPort>;
