using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.CompilerMessages;

using NUnit.Framework;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

[TestFixture]
public class AstIfStatementTest
{
    [Test]
    public void ConditionAndThenBodyTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;

        var visitor = new MockDefaultAstVisitor();
        var evaluator = new IfStatementEvaluator();

        var ast = new AstIfStatementNode
            {};

        evaluator.Evaluate( visitor, ast );

        Assert.AreEqual( 0, compilerMessageManger.Count( CompilerMessageLevel.Error ) );
    }
}

#region Work mock classes

public class MockIfStatementEvaluator : IConditionalStatementEvaluator<AstIfStatementNode>
{
    public IAstNode Evaluate( IAstVisitor visitor, AstIfStatementNode statement )
        => throw new NotImplementedException();
}

#endregion

public interface IConditionalStatementEvaluator<in TStatementNode> where TStatementNode : AstStatementNode
{
    IAstNode Evaluate( IAstVisitor visitor, TStatementNode statement );
}

public interface IIfStatementEvaluator : IConditionalStatementEvaluator<AstIfStatementNode> {}

public class IfStatementEvaluator : IIfStatementEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstIfStatementNode statement )
    {
        throw new NotImplementedException();
    }
}
