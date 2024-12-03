using System;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Events;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.UseCases.Analysis;

public sealed class SemanticAnalysisInputDataDetail(
    IEventEmitter eventEmitter,
    AstCompilationUnitNode compilationUnitNode,
    AggregateSymbolTable symbolTable )
{
    public IEventEmitter EventEmitter { get; } = eventEmitter;
    public AstCompilationUnitNode CompilationUnitNode { get; } = compilationUnitNode;
    public AggregateSymbolTable SymbolTable { get; } = symbolTable;
}

public sealed class SemanticAnalysisInputData( SemanticAnalysisInputDataDetail inputData )
    : IInputPort<SemanticAnalysisInputDataDetail>
{
    public SemanticAnalysisInputDataDetail InputData { get; } = inputData;
}

public sealed class SemanticAnalysisOutputDataDetail(
    AstCompilationUnitNode compilationUnitNode,
    AggregateSymbolTable symbolTable )
{
    public AstCompilationUnitNode CompilationUnitNode { get; } = compilationUnitNode;
    public AggregateSymbolTable SymbolTable { get; } = symbolTable;
}

public sealed class SemanticAnalysisOutputData( bool result, Exception? error, SemanticAnalysisOutputDataDetail outputData )
    : IOutputPort<SemanticAnalysisOutputDataDetail>
{
    public bool Result { get; } = result;
    public Exception? Error { get; } = error;
    public SemanticAnalysisOutputDataDetail OutputData { get; } = outputData;
}

public interface ISemanticAnalysisUseCase : IUseCase<SemanticAnalysisInputData, SemanticAnalysisOutputData> {}
