using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.UseCases.Analysis.Evaluations.Symbols;
using KSPCompiler.UseCases.Analysis.Obfuscators;

namespace KSPCompiler.Interactors.Analysis.Obfuscators;

public class ArrayElementEvaluator : IArrayElementEvaluator
{
    private StringBuilder Output { get; }

    public ArrayElementEvaluator( StringBuilder output )
    {
        Output = output;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstArrayElementExpressionNode expr )
    {
        expr.Left.Accept( visitor );

        Output.Append( '[' );

        expr.Right.Accept( visitor );

        Output.Append( ']' );

        return expr;
    }
}
