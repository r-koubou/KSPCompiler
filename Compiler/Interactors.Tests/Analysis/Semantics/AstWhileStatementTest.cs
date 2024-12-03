using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.CompilerMessages.Extensions;
using KSPCompiler.Domain.Events;
using KSPCompiler.Domain.Events.Extensions;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Interactors.Analysis.Semantics;
using KSPCompiler.Interactors.Tests.Commons;

using NUnit.Framework;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

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
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var visitor = new MockWhileStatementVisitor();
        var evaluator = new WhileStatementEvaluator( eventEmitter );

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

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( expectedErrorCount ) );
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
        // while() <-- FATAL: no condition (Syntax parser should not allow this)
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
