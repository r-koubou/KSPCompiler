using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Shared;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.EventEmitting;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions;

public sealed record ObfuscationInput
{
    public IEventEmitter EventEmitter { get; }
    public AstCompilationUnitNode CompilationUnitNode { get; }
    public AggregateSymbolTable SymbolTable { get; }

    public int DefaultOutputBufferCapacity { get; }

    // ReSharper disable once ConvertToPrimaryConstructor
    public ObfuscationInput(
        IEventEmitter eventEmitter,
        AstCompilationUnitNode compilationUnitNode,
        AggregateSymbolTable symbolTable,
        int defaultOutputBufferCapacity = 16384 )
    {
        EventEmitter                = eventEmitter;
        CompilationUnitNode         = compilationUnitNode;
        SymbolTable                 = symbolTable;
        DefaultOutputBufferCapacity = defaultOutputBufferCapacity;
    }
}

public sealed record ObfuscationOutput(
    string ObfuscatedScript
);

public interface IObfuscationUseCase
{
    Task<Result<ObfuscationOutput, CompilationFailureReason>> ExecuteAsync( ObfuscationInput input, CancellationToken cancellationToken = default );
}
