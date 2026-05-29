using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Compilation;
using KSPCompiler.Shared;
using KSPCompiler.Shared.Text;

namespace KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Definition;

public sealed record DefinitionInput(
    ICompilationCacheManager Cache,
    ScriptLocation Location,
    Position Position
)
{
    public ICompilationCacheManager Cache { get; } = Cache;
    public ScriptLocation Location { get; } = Location;
    public Position Position { get; } = Position;
}

public sealed record DefinitionOutput(
    List<LocationLink> Links
);

public interface IDefinitionHandlingUseCase
{
    Task<Result<DefinitionOutput, LanguageServerFailureReason>> ExecuteAsync( DefinitionInput input, CancellationToken cancellationToken = default );
}
