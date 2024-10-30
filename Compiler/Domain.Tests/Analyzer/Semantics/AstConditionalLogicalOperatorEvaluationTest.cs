using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols.MetaData;

using NUnit.Framework;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

[TestFixture]
public class AstConditionalLogicalOperatorEvaluationTest
{
    #region Common code implementation of each conditional logical operator

    private static void ConditionalLogicalOperatorTestBody<TOperatorNode>(
        Func<IAstVisitor, TOperatorNode, IAstNode> visit,
        int expectedErrorCount,
        AstExpressionNode left,
        AstExpressionNode right )
        where TOperatorNode : AstExpressionNode, new()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstConditionalLogicalOperatorVisitor();

        var conditionalBinaryOperatorEvaluator = new ConditionalLogicalOperatorEvaluator(
            compilerMessageManger
        );

        // left <opr> right
        var operatorNode = new TOperatorNode
        {
            Left  = left,
            Right = right
        };

        operatorNode.Left.Parent = operatorNode;

        visitor.Inject( conditionalBinaryOperatorEvaluator );

        var result = visit( visitor, operatorNode ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.AreEqual( expectedErrorCount, compilerMessageManger.Count( CompilerMessageLevel.Error ) );
        Assert.IsNotNull( result );
        Assert.AreEqual( DataTypeFlag.TypeBool, result?.TypeFlag );
    }

    private static void ConditionalLogicalOperatorTestBody<TOperatorNode>( Func<IAstVisitor, TOperatorNode, IAstNode> visit, int expectedErrorCount )
        where TOperatorNode : AstExpressionNode, new()
    {
        ConditionalLogicalOperatorTestBody(
            visit: visit,
            expectedErrorCount: 0,
            left: new AstEqualExpressionNode
            {
                TypeFlag = DataTypeFlag.TypeBool,
                Left     = new AstIntLiteralNode( 1 ),
                Right    = new AstIntLiteralNode( 1 )
            },
            right: new AstEqualExpressionNode
            {
                TypeFlag = DataTypeFlag.TypeBool,
                Left     = new AstIntLiteralNode( 2 ),
                Right    = new AstIntLiteralNode( 2 )
            }
        );
    }

    private static void CannotIncompatibleTypeConditionalLogicalOperatorTestBody<TOperatorNode>( Func<IAstVisitor, TOperatorNode, IAstNode> visit, int expectedErrorCount )
        where TOperatorNode : AstExpressionNode, new()
    {
        ConditionalLogicalOperatorTestBody(
            visit: visit,
            expectedErrorCount: 0,
            // 1 = 1 <opr> 2 + 2 <-- `2 + 2` is not boolean
            left: new AstEqualExpressionNode
            {
                TypeFlag = DataTypeFlag.TypeBool,
                Left     = new AstIntLiteralNode( 1 ),
                Right    = new AstIntLiteralNode( 1 )
            },
            right: new AstEqualExpressionNode
            {
                TypeFlag = DataTypeFlag.TypeBool,
                Left     = new AstIntLiteralNode( 2 ),
                Right    = new AstIntLiteralNode( 2 )
            }
        );
    }

    #endregion ~Common code implementation of each conditional logical operator

    [Test]
    public void LogicalAndConditionalOperatorTest()
        => ConditionalLogicalOperatorTestBody<AstLogicalAndExpressionNode>(
            ( visitor, node ) => visitor.Visit( node ),
            0
        );
}

#region Work mock classes

public class MockConditionalLogicalOperatorEvaluator : IConditionalLogicalOperatorEvaluator
{
    public IAstNode Evaluate( IAstVisitor<IAstNode> visitor, AstExpressionNode expr )
        => throw new NotImplementedException();
}

public class MockAstConditionalLogicalOperatorVisitor : DefaultAstVisitor
{
    private IConditionalLogicalOperatorEvaluator ConditionalBinaryOperatorEvaluator { get; set; } = new MockConditionalLogicalOperatorEvaluator();

    public void Inject( IConditionalLogicalOperatorEvaluator evaluator )
        => ConditionalBinaryOperatorEvaluator = evaluator;

    public override IAstNode Visit( AstLogicalOrExpressionNode node )
        => ConditionalBinaryOperatorEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstLogicalAndExpressionNode node )
        => ConditionalBinaryOperatorEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstLogicalXorExpressionNode node )
        => ConditionalBinaryOperatorEvaluator.Evaluate( this, node );
}

#endregion


public interface IConditionalLogicalOperatorEvaluator<TEvalResult>
{
    public TEvalResult Evaluate( IAstVisitor<TEvalResult> visitor, AstExpressionNode expr );
}

public interface IConditionalLogicalOperatorEvaluator : IConditionalLogicalOperatorEvaluator<IAstNode> {}


public class ConditionalLogicalOperatorEvaluator : IConditionalLogicalOperatorEvaluator
{
    private ICompilerMessageManger CompilerMessageManger { get; }

    public ConditionalLogicalOperatorEvaluator( ICompilerMessageManger compilerMessageManger )
    {
        CompilerMessageManger = compilerMessageManger;
    }

    public IAstNode Evaluate( IAstVisitor<IAstNode> visitor, AstExpressionNode expr )
        => throw new NotImplementedException();
}
