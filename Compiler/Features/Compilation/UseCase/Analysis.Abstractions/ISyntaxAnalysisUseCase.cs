using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.Compilation.Gateways.Parser;
using KSPCompiler.Shared;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions;

public sealed record SyntaxAnalysisInput(
    ISyntaxParser Parser
);

public sealed record SyntaxAnalysisOutput(
    AstCompilationUnitNode Node
);

public interface ISyntaxAnalysisUseCase
{
    Task<Result<SyntaxAnalysisOutput, CompilationFailureReason>> ExecuteAsync( SyntaxAnalysisInput input, CancellationToken cancellationToken = default );
}
