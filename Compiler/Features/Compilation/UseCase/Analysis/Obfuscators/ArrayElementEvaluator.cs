using System.Text;

using KSPCompiler.Features.Compilation.Domain.Ast.Nodes;
using KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Symbols;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;

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
