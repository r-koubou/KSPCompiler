using System;
using System.Collections.Generic;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Compilation;
using KSPCompiler.Shared.Text;
using KSPCompiler.Shared.UseCase;

namespace KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Renaming;

public sealed class RenamingInputPortDetail(
    ICompilationCacheManager cache,
    ScriptLocation location,
    Position position,
    string newName )
{
    public ICompilationCacheManager Cache { get; } = cache;
    public ScriptLocation Location { get; } = location;
    public Position Position { get; } = position;
    public string NewName { get; } = newName;
}

public sealed class RenamingInputPort(
    RenamingInputPortDetail input
) : InputPort<RenamingInputPortDetail>( input );

public sealed class RenamingOutputPort(
    Dictionary<ScriptLocation, List<RenamingItem>> changes,
    bool result,
    Exception? error = null
) : OutputPort<Dictionary<ScriptLocation, List<RenamingItem>>>( changes, result, error );

public interface IRenamingUseCase
    : IUseCase<RenamingInputPort, RenamingOutputPort>;
