using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Gateways.EventEmitting;

namespace KSPCompiler.UseCases.Analysis;

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

public sealed class PreprocessInputData : IInputPort<PreprocessInputDataDetail>
{
    public PreprocessInputDataDetail InputData { get; }

    public PreprocessInputData( PreprocessInputDataDetail inputData )
    {
        InputData = inputData;
    }
}

public interface IPreprocessUseCase : IUseCase<PreprocessInputData, UnitOutputPort> {}
