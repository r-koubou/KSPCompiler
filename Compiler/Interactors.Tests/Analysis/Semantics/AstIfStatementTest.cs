using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Interactors.Analysis.Semantics;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

[TestFixture]
public class AstIfStatementTest
{
    #region Common test methods

    private static void IfStatementEvaluationTestBody(
        AstExpressionNode condition,
        AstBlockNode codeBlock,
        AstBlockNode elseBlock,
        int expectedErrorCount )
    {
        var compilerMessageManger = ICompilerMessageManger.Default;

        var visitor = new MockIfStatementVisitor();
        var evaluator = new IfStatementEvaluator( compilerMessageManger );

        visitor.Inject( evaluator );

        var ast = new AstIfStatementNode
        {
            Condition = condition,
            CodeBlock = codeBlock,
            ElseBlock = elseBlock
        };

        ast.Condition.Parent = ast;
        ast.CodeBlock.Parent = ast;
        ast.ElseBlock.Parent = ast;

        evaluator.Evaluate( visitor, ast );

        compilerMessageManger.WriteTo( Console.Out );

        ClassicAssert.AreEqual( expectedErrorCount, compilerMessageManger.Count( CompilerMessageLevel.Error ) );
    }

    private static void IfStatementEvaluationTestBody( AstExpressionNode condition, int expectedErrorCount )
        => IfStatementEvaluationTestBody( condition, new AstBlockNode(), new AstBlockNode(), expectedErrorCount );

    #endregion

    [Test]
    public void ConditionEvaluationTest()
    {
        // if( 1 = 1 )
        // end if
        var condition = new AstEqualExpressionNode
        {
            // set an evaluated type value here because statement's unit test only here
            TypeFlag = DataTypeFlag.TypeBool,
            Left   = new AstIntLiteralNode( 1 ),
            Right  = new AstIntLiteralNode( 1 )
        };

        IfStatementEvaluationTestBody( condition, 0 );
    }

    [Test]
    public void ElseIfEvaluationTest()
    {
        // if( 1 = 2 )
        var condition = new AstEqualExpressionNode
        {
            // set an evaluated type value here because statement's unit test only here
            TypeFlag = DataTypeFlag.TypeBool,
            Left     = new AstIntLiteralNode( 1 ),
            Right    = new AstIntLiteralNode( 2 )
        };

        // else if( 2 = 2 )
        var elseBlock = new AstBlockNode();
        elseBlock.Statements.Add( new AstIfStatementNode
            {
                Condition = new AstEqualExpressionNode
                {
                    // set an evaluated type value here because statement's unit test only here
                    TypeFlag = DataTypeFlag.TypeBool,
                    Left     = new AstIntLiteralNode( 2 ),
                    Right    = new AstIntLiteralNode( 2 )
                },
                CodeBlock = new AstBlockNode()
            }
        );

        IfStatementEvaluationTestBody( condition, new AstBlockNode(), elseBlock, 0 );
    }

    [Test]
    public void CannotEvaluateWithoutConditionTest()
    {
        // if() <-- no condition
        // end if
        var condition = NullAstExpressionNode.Instance;

        IfStatementEvaluationTestBody( condition, 1 );
    }

    [Test]
    public void CannotEvaluateWithIncompatibleConditionTypeTest()
    {
        // if( 1 + 1 ) <-- incompatible condition type (its binary expression)
        // end if
        var condition = new AstAdditionExpressionNode
        {
            Left   = new AstIntLiteralNode( 1 ),
            Right  = new AstIntLiteralNode( 1 )
        };

        IfStatementEvaluationTestBody( condition, 1 );
    }

}
