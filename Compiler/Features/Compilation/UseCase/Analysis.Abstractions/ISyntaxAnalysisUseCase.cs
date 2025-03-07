using System;

using KSPCompiler.Features.Compilation.Gateways.Parsers;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks;
using KSPCompiler.Shared.UseCase;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions;

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
