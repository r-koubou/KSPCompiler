using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Declarations;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public class MockUserFunctionDeclarationEvaluator : IUserFunctionDeclarationEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstUserFunctionDeclarationNode node )
        => throw new System.NotImplementedException();
}
