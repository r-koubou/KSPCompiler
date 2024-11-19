using System.Text;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Interactors.Analysis.Obfuscators;
using KSPCompiler.Interactors.Analysis.Obfuscators.Extensions;

using NUnit.Framework;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

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

        Assert.AreEqual( expected, output.ToString() );
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

        Assert.AreEqual( expected, output.ToString() );
    }
}
