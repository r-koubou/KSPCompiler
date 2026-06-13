using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Shared;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.EventEmitting;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions;

public sealed record PreprocessInput(
    IEventEmitter EventEmitter,
    AstCompilationUnitNode CompilationUnitNode,
    AggregateSymbolTable SymbolTable
);

public interface IPreprocessUseCase
{
    Task<Result<Unit, CompilationFailureReason>> ExecuteAsync( PreprocessInput input, CancellationToken cancellationToken = default );
}
