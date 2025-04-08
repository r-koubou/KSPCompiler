using System;
using System.Text;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Operators;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Tests.Analysis.Obfuscators;

public class MockAstStringConcatenateOperatorVisitor : DefaultAstVisitor
{
    private StringBuilder Output { get; }
    private IStringConcatenateOperatorEvaluator Evaluator { get; set; } = new MockStringConcatenateOperatorEvaluator();

    public MockAstStringConcatenateOperatorVisitor( StringBuilder output )
    {
        Output = output;
    }

    public void Inject( IStringConcatenateOperatorEvaluator evaluator )
    {
        Evaluator = evaluator;
    }

    public override IAstNode Visit( AstStringConcatenateExpressionNode node )
        => Evaluator.Evaluate( this, node );

    public override IAstNode Visit( AstStringLiteralNode node )
    {
        Output.Append( node.Value );
        return node;
    }

    private class MockStringConcatenateOperatorEvaluator : IStringConcatenateOperatorEvaluator
    {
        public IAstNode Evaluate( IAstVisitor visitor, AstExpressionNode expr )
            => throw new NotImplementedException();
    }

}
