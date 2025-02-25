using System;
using System.Collections.Generic;

using KSPCompiler.Commons.Text;
using KSPCompiler.UseCases.LanguageServer.Compilation;

namespace KSPCompiler.UseCases.LanguageServer.Completion;

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
    CompletionHandlingInputPortDetail inputData
) : InputPort<CompletionHandlingInputPortDetail>( inputData );

public sealed class CompletionHandlingOutput(
    List<CompletionItem> outputData,
    bool result,
    Exception? error = null
) : OutputPort<List<CompletionItem>>( outputData, result, error );

public interface ICompletionHandlingUseCase
    : IUseCase<CompletionHandlingInputPort, CompletionHandlingOutput>;
