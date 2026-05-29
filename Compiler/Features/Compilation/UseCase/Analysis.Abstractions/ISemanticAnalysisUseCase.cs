using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Shared;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.EventEmitting;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions;

public sealed record SemanticAnalysisInput(
    IEventEmitter EventEmitter,
    AstCompilationUnitNode CompilationUnitNode,
    AggregateSymbolTable SymbolTable
);

public sealed record SemanticAnalysisOutput(
    AstCompilationUnitNode CompilationUnitNode,
    AggregateSymbolTable SymbolTable
);

public interface ISemanticAnalysisUseCase
{
    Task<Result<SemanticAnalysisOutput, CompilationFailureReason>> ExecuteAsync( SemanticAnalysisInput input, CancellationToken cancellationToken = default );
}
