using KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Features.Compilation.Domain.Symbols;
using KSPCompiler.Shared.EventEmitting;
using KSPCompiler.Shared.UseCase;

namespace KSPCompiler.Features.Compilation.UseCase.Abstractions;

public sealed class PreprocessInputDataDetail
{
    public IEventEmitter EventEmitter { get; }

    public AstCompilationUnitNode CompilationUnitNode { get; }

    public AggregateSymbolTable SymbolTable { get; }

    public PreprocessInputDataDetail(
        IEventEmitter eventEmitter,
        AstCompilationUnitNode compilationUnitNode,
        AggregateSymbolTable symbolTable )
    {
        EventEmitter        = eventEmitter;
        CompilationUnitNode = compilationUnitNode;
        SymbolTable         = symbolTable;
    }
}

public sealed class PreprocessInputData(
    PreprocessInputDataDetail inputInput
) : InputPort<PreprocessInputDataDetail>( inputInput );

public interface IPreprocessUseCase
    : IUseCase<PreprocessInputData, UnitOutputPort> {}
