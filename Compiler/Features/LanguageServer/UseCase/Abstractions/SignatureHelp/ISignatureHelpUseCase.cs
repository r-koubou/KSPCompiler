using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Compilation;
using KSPCompiler.Shared;
using KSPCompiler.Shared.Text;

namespace KSPCompiler.Features.LanguageServer.UseCase.Abstractions.SignatureHelp;

public sealed record SignatureHelpInput(
    ICompilationCacheManager Cache,
    ScriptLocation Location,
    Position Position
);

public sealed record SignatureHelpOutput(
    SignatureHelpItem? SignatureHelp
);

public interface ISignatureHelpUseCase
{
    Task<Result<SignatureHelpOutput, LanguageServerFailureReason>> ExecuteAsync( SignatureHelpInput input, CancellationToken cancellationToken = default );
}
