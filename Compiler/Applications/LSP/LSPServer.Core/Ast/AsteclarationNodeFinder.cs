using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.LSPServer.Core.Ast;

public class AsteclarationNodeFinder : DefaultAstVisitor
{
    private string variableName = string.Empty;
    private AstVariableDeclarationNode? variableDeclarationNode;

    private string userFunctionName = string.Empty;
    private AstUserFunctionDeclarationNode? userFunctionDeclarationNode;

    public bool TryFindVariableDeclarationNode( string name, AstCompilationUnitNode ast, out AstVariableDeclarationNode result )
    {
        variableName = name;
        variableDeclarationNode = null;

        ast.AcceptChildren( this );
        result = variableDeclarationNode!;

        return variableDeclarationNode != null;
    }

    public bool TryFindUserFunctionDeclarationNode( string name, AstCompilationUnitNode ast, out AstUserFunctionDeclarationNode result )
    {
        userFunctionName            = name;
        userFunctionDeclarationNode = null;

        ast.AcceptChildren( this );
        result = userFunctionDeclarationNode!;

        return userFunctionDeclarationNode != null;
    }

    public override IAstNode Visit( AstVariableDeclarationNode node )
    {
        if( node.Name == variableName )
        {
            variableDeclarationNode = node;
        }

        return base.Visit( node );
    }

    public override IAstNode Visit( AstUserFunctionDeclarationNode node )
    {
        if( node.Name == userFunctionName )
        {
            userFunctionDeclarationNode = node;
        }

        return base.Visit( node );
    }
}
