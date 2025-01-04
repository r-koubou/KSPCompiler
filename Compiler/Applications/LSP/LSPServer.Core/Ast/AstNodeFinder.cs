using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.LSPServer.Core.Ast;

public class AstNodeFinder : DefaultAstVisitor
{
    private string variableName = string.Empty;
    private AstVariableDeclarationNode? variableDeclarationNode;

    public bool TryFind( string name, AstCompilationUnitNode ast, out AstVariableDeclarationNode result )
    {
        variableName = name;
        variableDeclarationNode = null;

        ast.AcceptChildren( this );
        result = variableDeclarationNode!;

        return variableDeclarationNode != null;
    }

    public override IAstNode Visit( AstVariableDeclarationNode node )
    {
        if( node.Name == variableName )
        {
            variableDeclarationNode = node;
        }

        return base.Visit( node );
    }
}
