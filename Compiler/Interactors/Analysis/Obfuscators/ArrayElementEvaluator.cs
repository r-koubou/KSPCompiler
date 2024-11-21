using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.UseCases.Analysis.Evaluations.Symbols;

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
        Output.Append( '[' );

        expr.Left.Accept( visitor );

        Output.Append( ']' );

        return expr;
    }
}
