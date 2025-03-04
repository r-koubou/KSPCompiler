using System.Text;

using KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Symbols;
using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Expressions;

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
