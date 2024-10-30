using System;

using KSPCompiler.Domain.Ast.Analyzers;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Operators;
using KSPCompiler.Domain.Ast.Extensions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Extensions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Resources;

using NUnit.Framework;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

[TestFixture]
public class AstConditionalUnaryOperatorEvaluationTest
{
    [Test]
    public void ConditionalUnaryOperatorTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstConditionalUnaryOperatorVisitor();

        var conditionalBinaryOperatorEvaluator = new ConditionalUnaryOperatorEvaluator(
            compilerMessageManger
        );

        // not 1 = 1
        var operatorNode = new AstUnaryLogicalNotExpressionNode
        {
            Left  = new AstEqualExpressionNode
            {
                TypeFlag = DataTypeFlag.TypeBool,
                Left     = new AstIntLiteralNode( 1 ),
                Right    = new AstIntLiteralNode( 1 )
            }
        };
        operatorNode.Left.Parent = operatorNode;

        visitor.Inject( conditionalBinaryOperatorEvaluator );

        var result = visitor.Visit( operatorNode ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.AreEqual( 0, compilerMessageManger.Count( CompilerMessageLevel.Error ) );
        Assert.IsNotNull( result );
        Assert.AreEqual( DataTypeFlag.TypeBool, result?.TypeFlag );
    }

    [Test]
    public void CannotConditionalUnaryOperatorWithoutBooleanTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstConditionalUnaryOperatorVisitor();

        var conditionalBinaryOperatorEvaluator = new ConditionalUnaryOperatorEvaluator(
            compilerMessageManger
        );

        // not 1 + 1 <-- `1 + 1` is not conditional operator
        var operatorNode = new AstUnaryLogicalNotExpressionNode
        {
            Left  = new AstAdditionExpressionNode
            {
                Left  = new AstIntLiteralNode( 1 ),
                Right = new AstIntLiteralNode( 1 )
            }
        };
        operatorNode.Left.Parent = operatorNode;

        visitor.Inject( conditionalBinaryOperatorEvaluator );

        var result = visitor.Visit( operatorNode ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.AreEqual( 1, compilerMessageManger.Count( CompilerMessageLevel.Error ) );
        Assert.IsNotNull( result );
        Assert.AreEqual( DataTypeFlag.TypeBool, result?.TypeFlag );
    }
}

public class ConditionalUnaryOperatorEvaluator : IUnaryOperatorEvaluator
{
    private ICompilerMessageManger CompilerMessageManger { get; }

    private static AstExpressionNode CreateEvaluateNode( AstExpressionNode source, DataTypeFlag type )
    {
        var result = source.Clone<AstExpressionNode>();
        result.TypeFlag = type;

        return result;
    }

    public ConditionalUnaryOperatorEvaluator( ICompilerMessageManger compilerMessageManger )
        => CompilerMessageManger = compilerMessageManger;

    public IAstNode Evaluate( IAstVisitor<IAstNode> visitor, AstExpressionNode expr )
    {
        /*
          <<logical unary not operator>>
                      expr
                       +
                       |
          <<conditional binary operator>>
                    expr.Left
        */

        if( expr.ChildNodeCount != 1 || !expr.Id.IsBooleanSupportedUnaryOperator())
        {
            throw new AstAnalyzeException( expr, "Expected a unary logical not expression" );
        }

        if( expr.Left.Accept( visitor ) is not AstExpressionNode evaluatedLeft )
        {
            throw new AstAnalyzeException( expr, "Failed to evaluate left side of the unary logical not expression" );
        }

        if( !evaluatedLeft.TypeFlag.IsBoolean() )
        {
            CompilerMessageManger.Error(
                expr,
                CompilerMessageResources.semantic_error_unrayoprator_logicalnot_incompatible,
                evaluatedLeft.TypeFlag.ToMessageString()
            );

            // 上位のノードで評価を継続させるので代替のノードは生成しない
        }

        return CreateEvaluateNode( expr, DataTypeFlag.TypeBool );
    }
}

#region Working mock classes

public class MockConditionalUnaryOperatorEvaluator : IUnaryOperatorEvaluator
{
    public IAstNode Evaluate( IAstVisitor<IAstNode> visitor, AstExpressionNode expr )
        => throw new NotImplementedException();
}

public class MockAstConditionalUnaryOperatorVisitor : DefaultAstVisitor
{
    private IUnaryOperatorEvaluator ConditionalUnaryOperatorEvaluator { get; set; } = new MockConditionalUnaryOperatorEvaluator();

    public void Inject( IUnaryOperatorEvaluator evaluator )
        => ConditionalUnaryOperatorEvaluator = evaluator;

    public override IAstNode Visit( AstUnaryLogicalNotExpressionNode node )
        => ConditionalUnaryOperatorEvaluator.Evaluate( this, node );
}

#endregion
