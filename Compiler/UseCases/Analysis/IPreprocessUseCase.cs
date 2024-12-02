using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Events;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.UseCases.Analysis;

public sealed class PreprocessInputDataDetail(
    IEventDispatcher eventDispatcher,
    AstCompilationUnitNode compilationUnitNode,
    AggregateSymbolTable symbolTable )
{
    public IEventDispatcher EventDispatcher { get; } = eventDispatcher;
    public AstCompilationUnitNode CompilationUnitNode { get; } = compilationUnitNode;
    public AggregateSymbolTable SymbolTable { get; } = symbolTable;
}

public sealed class PreprocessInputData( PreprocessInputDataDetail inputData )
    : IInputPort<PreprocessInputDataDetail>
{
    public PreprocessInputDataDetail InputData { get; } = inputData;
}

public interface IPreprocessUseCase : IUseCase<PreprocessInputData, UnitOutputPort> {}
