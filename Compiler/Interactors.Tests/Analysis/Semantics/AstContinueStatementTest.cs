using System;

using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.CompilerMessages.Extensions;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Gateways.EventEmitting;
using KSPCompiler.Gateways.EventEmitting.Extensions;
using KSPCompiler.Interactors.Analysis.Semantics;
using KSPCompiler.Interactors.Tests.Commons;

using NUnit.Framework;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

[TestFixture]
public class AstContinueStatementTest
{
    #region Common test methods

    private static void ContinueStatementEvaluationTestBody( AstContinueStatementNode node, int expectedErrorCount )
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var visitor = new MockContinueStatementVisitor();
        var evaluator = new ContinueStatementEvaluator( eventEmitter );

        visitor.Inject( evaluator );

        evaluator.Evaluate( visitor, node );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( expectedErrorCount ) );
    }

    #endregion

    [Test]
    public void ConditionEvaluationTest()
    {
        // while( 1 # 0 )
        //    continue
        // end while
        var statement = new AstWhileStatementNode
        {
            Condition = new AstNotEqualExpressionNode
            {
                // set an evaluated type value here because statement's unit test only here
                TypeFlag = DataTypeFlag.TypeBool,
                Left     = new AstIntLiteralNode( 1 ),
                Right    = new AstIntLiteralNode( 0 )
            }
        };

        var continueStatement = new AstContinueStatementNode();
        statement.CodeBlock.Statements.Add( continueStatement );

        ContinueStatementEvaluationTestBody( continueStatement, 0 );
    }

    [Test]
    public void CannotEvaluateWithoutConditionTest()
    {
        // if( 1 = 1 )
        //    continue <-- continue statement available only in while loop
        // end if
        var statement = new AstIfStatementNode
        {
            Condition = new AstEqualExpressionNode
            {
                // set an evaluated type value here because statement's unit test only here
                TypeFlag = DataTypeFlag.TypeBool,
                Left     = new AstIntLiteralNode( 1 ),
                Right    = new AstIntLiteralNode( 1 )
            }
        };

        var continueStatement = new AstContinueStatementNode();
        statement.CodeBlock.Statements.Add( continueStatement );

        ContinueStatementEvaluationTestBody( continueStatement, 1 );
    }
}
