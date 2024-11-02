using System;

using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Conditionals;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols.MetaData;

using NUnit.Framework;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

[TestFixture]
public class AstWhileStatementTest
{
    #region Common test methods

    private static void WhileStatementEvaluationTestBody(
        AstExpressionNode condition,
        AstBlockNode codeBlock,
        int expectedErrorCount )
    {
        var compilerMessageManger = ICompilerMessageManger.Default;

        var visitor = new MockWhileStatementVisitor();
        var evaluator = new WhileStatementEvaluator( compilerMessageManger );

        visitor.Inject( evaluator );

        var ast = new AstWhileStatementNode
        {
            Condition = condition,
            CodeBlock = codeBlock
        };

        ast.Condition.Parent = ast;
        ast.CodeBlock.Parent = ast;

        evaluator.Evaluate( visitor, ast );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.AreEqual( expectedErrorCount, compilerMessageManger.Count( CompilerMessageLevel.Error ) );
    }

    private static void WhileStatementEvaluationTestBody( AstExpressionNode condition, int expectedErrorCount )
        => WhileStatementEvaluationTestBody( condition, new AstBlockNode(), expectedErrorCount );

    #endregion

    [Test]
    public void ConditionEvaluationTest()
    {
        // while( 1 # 1 )
        // end while
        var condition = new AstEqualExpressionNode
        {
            // set an evaluated type value here because statement's unit test only here
            TypeFlag = DataTypeFlag.TypeBool,
            Left   = new AstIntLiteralNode( 1 ),
            Right  = new AstIntLiteralNode( 1 )
        };

        WhileStatementEvaluationTestBody( condition, 0 );
    }

    [Test]
    public void CannotEvaluateWithoutConditionTest()
    {
        // while() <-- no condition
        // end while
        var condition = NullAstExpressionNode.Instance;

        WhileStatementEvaluationTestBody( condition, 1 );
    }

    [Test]
    public void CannotEvaluateWithIncompatibleConditionTypeTest()
    {
        // while( 1 + 1 ) <-- incompatible condition type (its binary expression)
        // end while
        var condition = new AstAdditionExpressionNode
        {
            Left   = new AstIntLiteralNode( 1 ),
            Right  = new AstIntLiteralNode( 1 )
        };

        WhileStatementEvaluationTestBody( condition, 1 );
    }

}

#region Work mock classes

public class MockWhileStatementVisitor : DefaultAstVisitor
{
    private IWhileStatementEvaluator Evaluator { get; set; } = new MockWhileStatementEvaluator();

    public void Inject( IWhileStatementEvaluator evaluator )
    {
        Evaluator = evaluator;
    }

    public override IAstNode Visit( AstWhileStatementNode node )
        => Evaluator.Evaluate( this, node );
}

public class MockWhileStatementEvaluator : IWhileStatementEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstWhileStatementNode statement )
        => throw new NotImplementedException();
}

#endregion

public interface IWhileStatementEvaluator : IConditionalStatementEvaluator<AstWhileStatementNode> {}

public class WhileStatementEvaluator : IWhileStatementEvaluator
{
    private ICompilerMessageManger CompilerMessageManger { get; }

    public WhileStatementEvaluator( ICompilerMessageManger compilerMessageManger )
    {
        CompilerMessageManger = compilerMessageManger;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstWhileStatementNode statement )
    {
        throw new NotImplementedException();
    }
}
