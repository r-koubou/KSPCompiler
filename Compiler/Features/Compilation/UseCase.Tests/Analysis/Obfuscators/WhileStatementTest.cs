using System.Text;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators.Extensions;
using KSPCompiler.Shared.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Shared.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Ast.Nodes.Statements;

using NUnit.Framework;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Obfuscators;

[TestFixture]
public class WhileStatementTest
{
    [Test]
    public void Test()
    {
        var expected = new StringBuilder()
                      .Append( $"while(1#2)" )
                      .NewLine()
                      .Append( "end while" )
                      .NewLine().ToString();

        var output = new StringBuilder();

        // while(1#2)
        // end while
        var node = new AstWhileStatementNode
        {
            Condition = new AstNotEqualExpressionNode
            {
                Left = new AstIntLiteralNode(1),
                Right = new AstIntLiteralNode(2)
            }
        };

        var whileEvaluator = new WhileStatementEvaluator( output );
        var visitor = new MockWhileStatementVisitor( output );

        visitor.Inject( whileEvaluator );
        visitor.Visit( node );

        Assert.That( output.ToString(), Is.EqualTo( expected ) );
    }

    [Test]
    public void ContinueTest()
    {
        var expected = new StringBuilder()
                      .Append( "while(1#2)" )
                      .NewLine()
                      .Append( "continue" )
                      .NewLine()
                      .Append( "end while" )
                      .NewLine().ToString();

        var output = new StringBuilder();

        // while(1#2)
        //   continue
        // end while
        var node = new AstWhileStatementNode
        {
            Condition = new AstNotEqualExpressionNode
            {
                Left  = new AstIntLiteralNode( 1 ),
                Right = new AstIntLiteralNode( 2 )
            },
            CodeBlock = new AstBlockNode
            {
                Statements =
                {
                    new AstContinueStatementNode()
                }
            }
        };

        var whileEvaluator = new WhileStatementEvaluator( output );
        var continueEvaluator = new ContinueStatementEvaluator( output );
        var visitor = new MockWhileStatementVisitor( output );

        visitor.Inject( whileEvaluator );
        visitor.Inject( continueEvaluator );
        visitor.Visit( node );

        Assert.That( output.ToString(), Is.EqualTo( expected ) );
    }
}
