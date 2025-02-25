using System;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Gateways.Parsers;

namespace KSPCompiler.UseCases.Analysis;

public class SyntaxAnalysisInputData(
    ISyntaxParser inputData
) : InputPort<ISyntaxParser>( inputData );

public sealed class SyntaxAnalysisOutputData(
    AstCompilationUnitNode outputData,
    bool result,
    Exception? error = null
) : OutputPort<AstCompilationUnitNode>( outputData, result, error );

public interface ISyntaxAnalysisUseCase
    : IUseCase<SyntaxAnalysisInputData, SyntaxAnalysisOutputData> {}
