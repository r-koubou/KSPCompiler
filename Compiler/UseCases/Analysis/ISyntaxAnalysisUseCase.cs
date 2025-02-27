using System;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Gateways.Parsers;

namespace KSPCompiler.UseCases.Analysis;

public class SyntaxAnalysisInputData(
    ISyntaxParser inputInput
) : InputPort<ISyntaxParser>( inputInput );

public sealed class SyntaxAnalysisOutputData(
    AstCompilationUnitNode outputData,
    bool result,
    Exception? error = null
) : OutputPort<AstCompilationUnitNode>( outputData, result, error );

public interface ISyntaxAnalysisUseCase
    : IUseCase<SyntaxAnalysisInputData, SyntaxAnalysisOutputData> {}
