using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Interactors.Analysis.Semantics;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

[TestFixture]
public class AstConditionalUnaryOperatorEvaluationTest
{
    [Test]
    public void ConditionalUnaryOperatorTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstConditionalUnaryOperatorVisitor();

        var conditionalBinaryOperatorEvaluator = new ConditionalUnaryOperatorEvaluator(
            compilerMessageManger,
            MockUtility.CreateBooleanConvolutionEvaluator( visitor )
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

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 0 ) );
        Assert.That( result, Is.Not.Null );
        Assert.That( result?.TypeFlag, Is.EqualTo( DataTypeFlag.TypeBool ) );
    }

    [Test]
    public void CannotConditionalUnaryOperatorWithoutBooleanTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstConditionalUnaryOperatorVisitor();

        var conditionalBinaryOperatorEvaluator = new ConditionalUnaryOperatorEvaluator(
            compilerMessageManger,
            MockUtility.CreateBooleanConvolutionEvaluator( visitor )
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

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 1 ) );
        Assert.That( result, Is.Not.Null );
        Assert.That( result?.TypeFlag, Is.EqualTo( DataTypeFlag.TypeBool ) );
    }
}
