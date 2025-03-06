using System;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Compilation;
using KSPCompiler.Shared.Text;
using KSPCompiler.Shared.UseCase;

namespace KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Hover;

public sealed class HoverInputPortDetail(
    ICompilationCacheManager cache,
    ScriptLocation location,
    Position position )
{
    public ICompilationCacheManager Cache { get; } = cache;
    public ScriptLocation Location { get; } = location;
    public Position Position { get; } = position;
}

public sealed class HoverInputPort(
    HoverInputPortDetail input
) : InputPort<HoverInputPortDetail>( input );

public sealed class HoverOutputPort(
    HoverItem? hoverItem,
    bool result,
    Exception? error = null
) : OutputPort<HoverItem?>( hoverItem, result, error );

public interface IHoverUseCase
    : IUseCase<HoverInputPort, HoverOutputPort>;
