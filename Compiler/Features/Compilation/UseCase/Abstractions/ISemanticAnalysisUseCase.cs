using System;

using KSPCompiler.Shared.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Shared.Domain.Symbols;
using KSPCompiler.Shared.EventEmitting;
using KSPCompiler.Shared.UseCase;

namespace KSPCompiler.Features.Compilation.UseCase.Abstractions;

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
    SemanticAnalysisInputDataDetail inputInput
) : InputPort<SemanticAnalysisInputDataDetail>( inputInput );

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
