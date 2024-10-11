using KSPCompiler.Domain.Ast.Analyzers.Evaluators;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.CompilerMessages;

namespace KSPCompiler.Domain.Ast.Analyzers.Semantics;

public class VariableInitializationEvaluator : IVariableInitializationEvaluator
{
    private IAstVisitor AstVisitor { get; }
    private ICompilerMessageManger CompilerMessageManger { get; }

    public VariableInitializationEvaluator( IAstVisitor astVisitor, ICompilerMessageManger compilerMessageManger )
    {
        AstVisitor            = astVisitor;
        CompilerMessageManger = compilerMessageManger;
    }

    public IAstNode Evaluate( IAstVisitor<IAstNode> visitor, AstExpressionNode expr, AbortTraverseToken abortTraverseToken )
    {
        throw new System.NotImplementedException();
    }
}
