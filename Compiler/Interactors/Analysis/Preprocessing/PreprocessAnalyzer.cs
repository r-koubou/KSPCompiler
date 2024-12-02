using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.Events;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases.Analysis;
using KSPCompiler.UseCases.Analysis.Evaluations.Preprocessing;

namespace KSPCompiler.Interactors.Analysis.Preprocessing;

public class PreprocessAnalyzer( IPreProcessorSymbolTable symbolTable, IEventDispatcher eventDispatcher )
    : DefaultAstVisitor, IAstTraversal
{
    private IPreprocessEvaluator Evaluator { get; } = new PreprocessEvaluator( symbolTable, eventDispatcher );

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
