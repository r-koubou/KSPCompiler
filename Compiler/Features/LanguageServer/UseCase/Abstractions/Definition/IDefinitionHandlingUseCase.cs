using System;
using System.Collections.Generic;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Compilation;
using KSPCompiler.Shared.Text;
using KSPCompiler.Shared.UseCase;

namespace KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Definition;

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
    DefinitionInputPortDetail input
) : InputPort<DefinitionInputPortDetail>( input );

public sealed class DefinitionOutputPort(
    List<LocationLink> links,
    bool result,
    Exception? error = null
) : OutputPort<List<LocationLink>>( links, result, error );

public interface IDefinitionHandlingUseCase
    : IUseCase<DefinitionInputPort, DefinitionOutputPort>;
