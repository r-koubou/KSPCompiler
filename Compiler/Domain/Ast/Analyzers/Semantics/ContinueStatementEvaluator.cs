using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Conditionals;
using KSPCompiler.Domain.Ast.Extensions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Resources;

namespace KSPCompiler.Domain.Ast.Analyzers.Semantics;

public class ContinueStatementEvaluator : IContinueStatementEvaluator
{
    private ICompilerMessageManger CompilerMessageManger { get; }

    public ContinueStatementEvaluator( ICompilerMessageManger compilerMessageManger )
    {
        CompilerMessageManger = compilerMessageManger;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstContinueStatementNode statement )
    {
        if( !statement.HasParent<AstWhileStatementNode>() )
        {
            CompilerMessageManger.Error(
                statement,
                CompilerMessageResources.semantic_error_continue_invalid
            );
        }

        return statement.Clone<AstContinueStatementNode>();
    }
}
