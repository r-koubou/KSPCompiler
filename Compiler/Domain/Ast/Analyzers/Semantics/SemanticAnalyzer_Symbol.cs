using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Ast.Analyzers.Semantics;

public partial class SemanticAnalyzer
{
    public override IAstNode Visit( AstSymbolExpressionNode node, AbortTraverseToken abortTraverseToken )
        => SymbolEvaluator.Evaluate( this, node, abortTraverseToken );

    public override IAstNode Visit( AstArrayElementExpressionNode node, AbortTraverseToken abortTraverseToken )
        => ArrayElementEvaluator.Evaluate( this, node, abortTraverseToken );
}
