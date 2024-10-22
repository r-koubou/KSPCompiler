using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Declarations;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public class MockCallbackDeclarationEvaluator : ICallbackDeclarationEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstCallbackDeclarationNode node )
        => throw new System.NotImplementedException();
}
