using System;
using System.Collections.Generic;

using KSPCompiler.Commons.Text;
using KSPCompiler.UseCases.LanguageServer.Compilation;

namespace KSPCompiler.UseCases.LanguageServer.Hover;

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
    HoverInputPortDetail handlingInputData
) : InputPort<HoverInputPortDetail>( handlingInputData );

public sealed class HoverOutputPort(
    HoverItem? hoverItem,
    bool result,
    Exception? error = null
) : OutputPort<HoverItem?>( hoverItem, result, error );

public interface IHoverUseCase
    : IUseCase<HoverInputPort, HoverOutputPort>;
