using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Interactor.Analysis.Commons.Extensions;
using KSPCompiler.Resources;
using KSPCompiler.UseCases.Analysis.Evaluations.Conditionals;

namespace KSPCompiler.Interactor.Analysis.Semantics;

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
