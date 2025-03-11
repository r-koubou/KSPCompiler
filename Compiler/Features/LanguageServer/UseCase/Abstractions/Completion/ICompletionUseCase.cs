using System;
using System.Collections.Generic;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Compilation;
using KSPCompiler.Shared.Text;
using KSPCompiler.Shared.UseCase;

namespace KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Completion;

public sealed class CompletionHandlingInputPortDetail(
    ICompilationCacheManager compilerCacheService,
    ScriptLocation scriptLocation,
    Position position
)
{
    public ICompilationCacheManager Cache { get; } = compilerCacheService;
    public ScriptLocation Location { get; } = scriptLocation;
    public Position Position { get; } = position;
}

public sealed class CompletionHandlingInputPort(
    CompletionHandlingInputPortDetail inputInput
) : InputPort<CompletionHandlingInputPortDetail>( inputInput );

public sealed class CompletionHandlingOutput(
    List<CompletionItem> outputData,
    bool result,
    Exception? error = null
) : OutputPort<List<CompletionItem>>( outputData, result, error );

public interface ICompletionUseCase
    : IUseCase<CompletionHandlingInputPort, CompletionHandlingOutput>;
