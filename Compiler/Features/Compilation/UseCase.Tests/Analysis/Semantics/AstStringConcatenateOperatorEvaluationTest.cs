using System;

using KSPCompiler.Features.Compilation.Domain.Messages;
using KSPCompiler.Features.Compilation.Domain.Messages.Extensions;
using KSPCompiler.Features.Compilation.Gateways.EventEmitting;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics;
using KSPCompiler.Features.Compilation.UseCase.Tests.Commons;
using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Symbols.MetaData;
using KSPCompiler.Shared.EventEmitting.Extensions;

using NUnit.Framework;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Semantics;

[TestFixture]
public class AstStringConcatenateOperatorEvaluationTest
{
    #region Common test methods

    private static void ConcatenateOperatorTestBody( AstExpressionNode left, AstExpressionNode right, int expectedErrorCount )
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var visitor = new MockAstStringConcatenateOperatorVisitor();

        var evaluator = new StringConcatenateOperatorEvaluator(
            eventEmitter,
            new MockStringConvolutionEvaluator()
        );

        visitor.Inject( evaluator );

        var operatorNode = new AstStringConcatenateExpressionNode
        {
            Left  = left,
            Right = right
        };

        visitor.Visit( operatorNode );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count(), Is.EqualTo( expectedErrorCount ) );
    }

    #endregion

    [Test]
    public void StringConcatenateTest()
    {
        // "abc" & "def"
        ConcatenateOperatorTestBody(
            new AstStringLiteralNode( "abc" ),
            new AstStringLiteralNode( "def" ),
            0
        );
    }

    [Test]
    public void StringAndIntegerConcatenateTest()
    {
        // "abc" & 1
        ConcatenateOperatorTestBody(
            new AstStringLiteralNode( "abc" ),
            new AstIntLiteralNode( 1 ),
            0
        );
    }

    [Test]
    public void NonStringConcatenateTest()
    {
        // 1 & 1
        ConcatenateOperatorTestBody(
            new AstIntLiteralNode( 1 ),
            new AstIntLiteralNode( 1 ),
            0
        );
    }

    [Test]
    public void CannotConditionalAndStringConcatenateTest()
    {
        // ( 1 > 0 ) & "abc" <-- conditional (boolean) is not allowed
        ConcatenateOperatorTestBody(
            new AstGreaterThanExpressionNode
            {
                // make a evaluated node
                // set an evaluated type value here because concatenating unit test only here
                TypeFlag = DataTypeFlag.TypeBool,
                Left     = new AstIntLiteralNode( 1 ),
                Right    = new AstIntLiteralNode( 0 )
            },
            new AstStringLiteralNode( "abc" ),
            1
        );
    }
}
