using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Compilation;
using KSPCompiler.Shared;
using KSPCompiler.Shared.Text;

namespace KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Completion;

public sealed record CompletionHandlingInput(
    ICompilationCacheManager Cache,
    ScriptLocation Location,
    Position Position,
    bool PreferSnippetInsertion
);

public sealed record CompletionHandlingOutput(
    List<CompletionItem> OutputData
);

public interface ICompletionUseCase
{
    Task<Result<CompletionHandlingOutput, LanguageServerFailureReason>> ExecuteAsync( CompletionHandlingInput input, CancellationToken cancellationToken = default );
}
