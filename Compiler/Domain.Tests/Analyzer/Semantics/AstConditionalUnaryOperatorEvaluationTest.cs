using System;

using KSPCompiler.Domain.Ast.Analyzers.Semantics;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols.MetaData;

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
