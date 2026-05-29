using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Compilation;
using KSPCompiler.Shared;
using KSPCompiler.Shared.Text;

namespace KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Renaming;

public sealed record RenamingInput(
    ICompilationCacheManager Cache,
    ScriptLocation Location,
    Position Position,
    string NewName
);

public sealed record RenamingOutput(
    Dictionary<ScriptLocation, List<RenamingItem>> Changes
);

public interface IRenamingUseCase
{
    Task<Result<RenamingOutput, LanguageServerFailureReason>> ExecuteAsync( RenamingInput input, CancellationToken cancellationToken = default );
}
