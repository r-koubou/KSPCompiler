using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.UseCases.Analysis.Evaluations.Operators;

namespace KSPCompiler.Interactor.Tests.Analysis.Semantics;

public class MockAssignOperatorEvaluator : IAssignOperatorEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstExpressionNode expr )
    {
        throw new NotImplementedException();
    }
}
