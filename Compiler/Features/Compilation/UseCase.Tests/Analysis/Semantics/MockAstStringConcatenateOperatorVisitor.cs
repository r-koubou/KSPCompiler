using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Operators;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Tests.Analysis.Semantics;

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
