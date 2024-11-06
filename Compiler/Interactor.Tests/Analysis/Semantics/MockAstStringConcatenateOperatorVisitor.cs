using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.UseCases.Analysis.Evaluations.Operators;

namespace KSPCompiler.Interactor.Tests.Analysis.Semantics;

public class MockAstStringConcatenateOperatorVisitor : DefaultAstVisitor
{
    private IStringConcatenateOperatorEvaluator StringConcatenateOperatorEvaluator { get; set; } = new MockStringConcatenateOperatorEvaluator();

    public void Inject( IStringConcatenateOperatorEvaluator unaryOperatorEvaluator )
    {
        StringConcatenateOperatorEvaluator = unaryOperatorEvaluator;
    }

    public override IAstNode Visit( AstStringConcatenateExpressionNode node )
        => StringConcatenateOperatorEvaluator.Evaluate( this, node );
}
