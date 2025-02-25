using System;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Gateways.EventEmitting;

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

public sealed class SemanticAnalysisInputData(
    SemanticAnalysisInputDataDetail inputData
) : InputPort<SemanticAnalysisInputDataDetail>( inputData );

public sealed class SemanticAnalysisOutputDataDetail(
    AstCompilationUnitNode compilationUnitNode,
    AggregateSymbolTable symbolTable )
{
    public AstCompilationUnitNode CompilationUnitNode { get; } = compilationUnitNode;

    public AggregateSymbolTable SymbolTable { get; } = symbolTable;
}

public sealed class SemanticAnalysisOutputData(
    SemanticAnalysisOutputDataDetail outputData,
    bool result,
    Exception? error
) : OutputPort<SemanticAnalysisOutputDataDetail>( outputData, result, error );

public interface ISemanticAnalysisUseCase
    : IUseCase<SemanticAnalysisInputData, SemanticAnalysisOutputData> {}
