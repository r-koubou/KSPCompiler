using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.EventEmitting;
using KSPCompiler.Shared.UseCase;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions;

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
