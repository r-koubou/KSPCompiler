using System.Text;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Interactors.Analysis.Obfuscators;
using KSPCompiler.Interactors.Analysis.Obfuscators.Extensions;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

[TestFixture]
public class IfStatementTest
{
    [Test]
    public void Test()
    {
        var expected = new StringBuilder()
                      .Append( $"if(1=2)" )
                      .NewLine()
                      .Append( "end if" )
                      .NewLine().ToString();

        var output = new StringBuilder();

        // if(1=2)
        // end if
        var node = new AstIfStatementNode
        {
            Condition = new AstEqualExpressionNode
            {
                Left = new AstIntLiteralNode(1),
                Right = new AstIntLiteralNode(2)
            }
        };

        var evaluator = new IfStatementEvaluator( output );
        var visitor = new MockIfStatementVisitor( output );

        visitor.Inject( evaluator );
        visitor.Visit( node );

        ClassicAssert.AreEqual( expected, output.ToString() );
    }

    [Test]
    public void ElseTest()
    {
        var expected = new StringBuilder()
                      .Append( $"if(1=2)" )
                      .NewLine()
                      .Append( "else" )
                      .NewLine()
                      .Append( "message()" )
                      .NewLine()
                      .Append( "end if" )
                      .NewLine().ToString();

        var output = new StringBuilder();

        // if(1=2)
        // else
        //   message()
        // end if
        var node = new AstIfStatementNode
        {
            Condition = new AstEqualExpressionNode
            {
                Left  = new AstIntLiteralNode( 1 ),
                Right = new AstIntLiteralNode( 2 )
            },
            ElseBlock = new AstBlockNode
            {
                Statements =
                {
                    new AstCallCommandExpressionNode
                    {
                        Left = new AstSymbolExpressionNode( "message" )
                    }
                }
            }
        };

        var evaluator = new IfStatementEvaluator( output );
        var visitor = new MockIfStatementVisitor( output );

        visitor.Inject( evaluator );
        visitor.Visit( node );

        ClassicAssert.AreEqual( expected, output.ToString() );
    }

}
