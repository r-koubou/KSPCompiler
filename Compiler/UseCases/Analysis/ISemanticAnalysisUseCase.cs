using System;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Gateways.EventEmitting;

namespace KSPCompiler.UseCases.Analysis;

public sealed class SemanticAnalysisInputDataDetail
{
    public IEventEmitter EventEmitter { get; }

    public AstCompilationUnitNode CompilationUnitNode { get; }

    public AggregateSymbolTable SymbolTable { get; }

    public SemanticAnalysisInputDataDetail(
        IEventEmitter eventEmitter,
        AstCompilationUnitNode compilationUnitNode,
        AggregateSymbolTable symbolTable )
    {
        EventEmitter        = eventEmitter;
        CompilationUnitNode = compilationUnitNode;
        SymbolTable         = symbolTable;
    }
}

public sealed class SemanticAnalysisInputData : IInputPort<SemanticAnalysisInputDataDetail>
{
    public SemanticAnalysisInputDataDetail InputData { get; }

    public SemanticAnalysisInputData( SemanticAnalysisInputDataDetail inputData )
    {
        InputData = inputData;
    }
}

public sealed class SemanticAnalysisOutputDataDetail
{
    public AstCompilationUnitNode CompilationUnitNode { get; }

    public AggregateSymbolTable SymbolTable { get; }

    public SemanticAnalysisOutputDataDetail(
        AstCompilationUnitNode compilationUnitNode,
        AggregateSymbolTable symbolTable )
    {
        CompilationUnitNode = compilationUnitNode;
        SymbolTable         = symbolTable;
    }
}

public sealed class SemanticAnalysisOutputData( bool result, Exception? error, SemanticAnalysisOutputDataDetail outputData )
    : IOutputPort<SemanticAnalysisOutputDataDetail>
{
    public bool Result { get; } = result;
    public Exception? Error { get; } = error;
    public SemanticAnalysisOutputDataDetail OutputData { get; } = outputData;
}

public interface ISemanticAnalysisUseCase : IUseCase<SemanticAnalysisInputData, SemanticAnalysisOutputData> {}
