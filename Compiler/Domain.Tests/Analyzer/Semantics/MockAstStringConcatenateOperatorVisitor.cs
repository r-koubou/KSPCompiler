using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Operators;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public class MockAstStringConcatenateOperatorVisitor : DefaultAstVisitor
{
    private IStringConcatenateOperatorEvaluator StringConcatenateOperatorEvaluator { get; set; } = new MockStringConcatenateOperatorEvaluator();

    public void Inject( IStringConcatenateOperatorEvaluator unaryOperatorEvaluator )
    {
        StringConcatenateOperatorEvaluator = unaryOperatorEvaluator;
    }

    public override IAstNode Visit( AstStringConcatenateExpressionNode node, AbortTraverseToken abortTraverseToken )
        => StringConcatenateOperatorEvaluator.Evaluate( this, node, abortTraverseToken );
}
