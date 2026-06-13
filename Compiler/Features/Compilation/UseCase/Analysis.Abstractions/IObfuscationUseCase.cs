using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Shared;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.EventEmitting;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions;

public sealed record ObfuscationInput(
    IEventEmitter EventEmitter,
    AstCompilationUnitNode CompilationUnitNode,
    AggregateSymbolTable SymbolTable,
    int DefaultOutputBufferCapacity = 16384
);

public sealed record ObfuscationOutput(
    string ObfuscatedScript
);

public interface IObfuscationUseCase
{
    Task<Result<ObfuscationOutput, CompilationFailureReason>> ExecuteAsync( ObfuscationInput input, CancellationToken cancellationToken = default );
}
