using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Ast.Analyzers.Semantics;

public partial class SemanticAnalyzer
{
    public override IAstNode Visit( AstSymbolExpressionNode node )
        => SymbolEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstArrayElementExpressionNode node )
        => ArrayElementEvaluator.Evaluate( this, node );
}
