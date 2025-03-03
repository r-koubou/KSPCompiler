using KSPCompiler.Features.Compilation.Domain.Ast.Nodes;
using KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Statements;
using KSPCompiler.Features.Compilation.Domain.Symbols;
using KSPCompiler.Features.Compilation.UseCase.Abstractions;
using KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Preprocessing;
using KSPCompiler.Shared.EventEmitting;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Preprocessing;

public class PreprocessAnalyzer( IPreProcessorSymbolTable symbolTable, IEventEmitter eventEmitter )
    : DefaultAstVisitor, IAstTraversal
{
    private IPreprocessEvaluator Evaluator { get; } = new PreprocessEvaluator( symbolTable, eventEmitter );

    public void Traverse( AstCompilationUnitNode node )
    {
        node.AcceptChildren( this );
    }

    public override IAstNode Visit( AstPreprocessorDefineNode node )
        => Evaluator.Evaluate( this, node );

    public override IAstNode Visit( AstPreprocessorUndefineNode node )
        => Evaluator.Evaluate( this, node );

    public override IAstNode Visit( AstPreprocessorIfdefineNode node )
        => Evaluator.Evaluate( this, node );

    public override IAstNode Visit( AstPreprocessorIfnotDefineNode node )
        => Evaluator.Evaluate( this, node );
}
