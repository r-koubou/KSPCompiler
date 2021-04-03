// Generated by CodeGenerators/ASTCodeGenerator

using KSPCompiler.Domain.Ast.Blocks;
using KSPCompiler.Domain.Ast.Statements;
using KSPCompiler.Domain.Ast.Expressions;

namespace KSPCompiler.Domain.Ast
{
    public interface IAstVisitor<out T>
    {
        T Visit( AstCompilationUnit node );
        T Visit( AstCallbackDeclaration node );
        T Visit( AstUserFunctionDeclaration node );
        T Visit( AstArgument node );
        T Visit( AstArgumentList node );
        T Visit( AstBlock node );
        T Visit( AstCaseBlock node );
        T Visit( AstVariableDeclaration node );
        T Visit( AstVariableInitializer node );
        T Visit( AstPrimitiveInitializer node );
        T Visit( AstArrayInitializer node );
        T Visit( AstIfStatement node );
        T Visit( AstWhileStatement node );
        T Visit( AstSelectStatement node );
        T Visit( AstCallKspUserFunctionStatement node );
        T Visit( AstKspPreprocessorDefine node );
        T Visit( AstKspPreprocessorUndefine node );
        T Visit( AstKspPreprocessorIfdefine node );
        T Visit( AstKspPreprocessorIfnotDefine node );

        T Visit( IAstNode node )
        {
            return Visit( (dynamic)node );
        }
    }
}
