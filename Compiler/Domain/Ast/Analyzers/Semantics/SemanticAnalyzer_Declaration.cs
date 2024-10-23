using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Domain.Ast.Analyzers.Semantics;

public partial class SemanticAnalyzer
{
    public override IAstNode Visit( AstCallbackDeclarationNode node )
        => CallbackDeclarationEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstUserFunctionDeclarationNode node )
        => UserFunctionDeclarationEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstVariableDeclarationNode node )
        => VariableDeclarationEvaluator.Evaluate( this, node );
}
