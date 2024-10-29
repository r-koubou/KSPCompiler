using System;

using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Operators;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;

using NUnit.Framework;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

[TestFixture]
public class AstConditionalBinaryOperatorEvaluationTest
{

    [Test]
    public void EqualConditionalOperatorTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstConditionalBinaryOperatorVisitor();

        var conditionalBinaryOperatorEvaluator = new ConditionalBinaryOperatorEvaluator(
            compilerMessageManger
        );

        var operatorNode = new AstEqualExpressionNode
        {
            Left  = new AstIntLiteralNode( 1 ),
            Right = new AstIntLiteralNode( 1 )
        };

        visitor.Inject( conditionalBinaryOperatorEvaluator );

        var result = visitor.Visit( operatorNode ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.AreEqual( 0, compilerMessageManger.Count( CompilerMessageLevel.Error ) );
        Assert.IsNotNull( result );
        Assert.AreEqual( DataTypeFlag.TypeBool, result?.TypeFlag );
    }
}

public class ConditionalBinaryOperatorEvaluator : IBinaryOperatorEvaluator
{
    private ICompilerMessageManger CompilerMessageManger { get; }

    public ConditionalBinaryOperatorEvaluator( ICompilerMessageManger compilerMessageManger )
    {
        CompilerMessageManger = compilerMessageManger;
    }

    public IAstNode Evaluate( IAstVisitor<IAstNode> visitor, AstExpressionNode expr )
    {
        throw new NotImplementedException();
    }
}

#region Working Mock classes

public class MockConditionalBinaryOperatorEvaluator : IBinaryOperatorEvaluator
{
    public IAstNode Evaluate( IAstVisitor<IAstNode> visitor, AstExpressionNode expr )
        => throw new NotImplementedException();
}

public class MockAstConditionalBinaryOperatorVisitor : DefaultAstVisitor
{
    private IBinaryOperatorEvaluator ConditionalBinaryOperatorEvaluator { get; set; } = new MockConditionalBinaryOperatorEvaluator();

    public void Inject( IBinaryOperatorEvaluator evaluator )
        => ConditionalBinaryOperatorEvaluator = evaluator;

    public override IAstNode Visit( AstEqualExpressionNode node )
        => ConditionalBinaryOperatorEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstNotEqualExpressionNode node )
        => ConditionalBinaryOperatorEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstLessThanExpressionNode node )
        => ConditionalBinaryOperatorEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstGreaterThanExpressionNode node )
        => ConditionalBinaryOperatorEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstLessEqualExpressionNode node )
        => ConditionalBinaryOperatorEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstGreaterEqualExpressionNode node )
        => ConditionalBinaryOperatorEvaluator.Evaluate( this, node );
}

#endregion
