using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Compilation;
using KSPCompiler.Shared;
using KSPCompiler.Shared.Text;

namespace KSPCompiler.Features.LanguageServer.UseCase.Abstractions.FindReferences;

public sealed record FindReferenceInput(
    ICompilationCacheManager Cache,
    ScriptLocation Location,
    Position Position
);

public sealed record FindReferenceOutput(
    List<ReferenceItem> References
);

public interface IFindReferenceUseCase
{
    Task<Result<FindReferenceOutput, LanguageServerFailureReason>> ExecuteAsync( FindReferenceInput input, CancellationToken cancellationToken = default );
}
