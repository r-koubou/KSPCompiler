using System;

using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Interactor.Analysis.Semantics;

using NUnit.Framework;

namespace KSPCompiler.Interactor.Tests.Analysis.Semantics;

[TestFixture]
public class AstContinueStatementTest
{
    #region Common test methods

    private static void ContinueStatementEvaluationTestBody( AstContinueStatementNode node, int expectedErrorCount )
    {
        var compilerMessageManger = ICompilerMessageManger.Default;

        var visitor = new MockContinueStatementVisitor();
        var evaluator = new ContinueStatementEvaluator( compilerMessageManger );

        visitor.Inject( evaluator );

        evaluator.Evaluate( visitor, node );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.AreEqual( expectedErrorCount, compilerMessageManger.Count( CompilerMessageLevel.Error ) );
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
