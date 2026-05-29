using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Compilation;
using KSPCompiler.Shared;

namespace KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Folding;

public sealed record FoldingRangeInput(
    ICompilationCacheManager Cache,
    ScriptLocation Location
);

public sealed record FoldingRangeOutput(
    List<FoldingItem> Ranges
);

public interface IFoldingRangeUseCase
{
    Task<Result<FoldingRangeOutput, LanguageServerFailureReason>> ExecuteAsync( FoldingRangeInput input, CancellationToken cancellationToken = default );
}
