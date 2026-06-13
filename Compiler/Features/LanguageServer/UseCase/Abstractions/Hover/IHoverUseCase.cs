using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Compilation;
using KSPCompiler.Shared;
using KSPCompiler.Shared.Text;

namespace KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Hover;

public sealed record HoverInput(
    ICompilationCacheManager Cache,
    ScriptLocation Location,
    Position Position
);

public sealed record HoverOutput(
    HoverItem? HoverItem
);

public interface IHoverUseCase
{
    Task<Result<HoverOutput, LanguageServerFailureReason>> ExecuteAsync( HoverInput input, CancellationToken cancellationToken = default );
}
