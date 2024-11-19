using System.Text;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Interactors.Analysis.Obfuscators;
using KSPCompiler.Interactors.Analysis.Obfuscators.Extensions;

using NUnit.Framework;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

[TestFixture]
public class SelectStatementTest
{
    [Test]
    public void Test()
    {
        var expected = new StringBuilder()
                      .Append( "select($x)" )
                      .NewLine()
                      .Append( "case 1" )
                      .NewLine()
                      .Append( "message()" )
                      .NewLine()
                      .Append( "case 2 to 5" )
                      .NewLine()
                      .Append( "message()" )
                      .NewLine()
                      .Append( "end select" )
                      .NewLine().ToString();

        var output = new StringBuilder();

        // select($x)
        //   case 1
        //     message()
        //   case 2 to 5
        //     message()
        // end select
        var node = new AstSelectStatementNode
        {
            Condition = new AstSymbolExpressionNode( "$x" ),
            CaseBlocks =
            {
                new AstCaseBlock
                {
                    ConditionFrom = new AstIntLiteralNode( 1 ),
                    CodeBlock = new AstBlockNode
                    {
                        Statements =
                        {
                            new AstCallCommandExpressionNode
                            {
                                Left = new AstSymbolExpressionNode( "message" )
                            }
                        }
                    }
                },
                new AstCaseBlock
                {
                    ConditionFrom = new AstIntLiteralNode( 2 ),
                    ConditionTo = new AstIntLiteralNode( 5 ),
                    CodeBlock = new AstBlockNode
                    {
                        Statements =
                        {
                            new AstCallCommandExpressionNode
                            {
                                Left = new AstSymbolExpressionNode( "message" )
                            }
                        }
                    }
                }
            }
        };

        var whileEvaluator = new SelectStatementEvaluator( output );
        var visitor = new MockSelectStatementVisitor( output );

        visitor.Inject( whileEvaluator );
        visitor.Visit( node );

        Assert.AreEqual( expected, output.ToString() );
    }
}
