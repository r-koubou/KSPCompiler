using System;

using KSPCompiler.Features.Compilation.Domain.Messages;
using KSPCompiler.Features.Compilation.Domain.Messages.Extensions;
using KSPCompiler.Features.Compilation.Gateways.EventEmitting;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics;
using KSPCompiler.Features.Compilation.UseCase.Tests.Commons;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Statements;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;
using KSPCompiler.Shared.EventEmitting.Extensions;

using NUnit.Framework;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Semantics;

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
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var visitor = new MockIfStatementVisitor();
        var evaluator = new IfStatementEvaluator( eventEmitter );

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

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( expectedErrorCount ) );
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
