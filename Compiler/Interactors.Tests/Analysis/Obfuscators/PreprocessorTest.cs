using System.Text;

using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Interactors.Analysis.Obfuscators;
using KSPCompiler.Interactors.Analysis.Obfuscators.Extensions;

using NUnit.Framework;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

[TestFixture]
public class PreprocessorTest
{
    [Test]
    public void IfDefTest()
    {
        // USE_CODE_IF(DEMO)
        // message()
        // END_USE_CODE

        const string preprocessSymbolName = "DEMO";
        var expected = new StringBuilder()
                      .Append("USE_CODE_IF(")
                      .Append( preprocessSymbolName )
                      .Append( ')' )
                      .NewLine()
                      .Append( "message()" )
                      .NewLine()
                      .Append( "END_USE_CODE" )
                      .NewLine()
                      .ToString();

        var output = new StringBuilder();

        var ifDefineNode = new AstPreprocessorIfdefineNode
        {
            Condition = new AstSymbolExpressionNode( preprocessSymbolName )
        };

        ifDefineNode.Block.Statements.Add(
            new AstCallCommandExpressionNode
            {
                Left = new AstSymbolExpressionNode( "message" )
            }
        );

        var evaluator = new PreprocessEvaluator( output );
        var visitor = new MockPreprocessEvaluatorVisitor( output );

        visitor.Inject( evaluator );
        visitor.Visit( ifDefineNode );

        Assert.AreEqual( expected, output.ToString() );
    }

    [Test]
    public void IfNotDefTest()
    {
        // USE_CODE_IF_NOT(DEMO)
        // message()
        // END_USE_CODE

        const string preprocessSymbolName = "DEMO";
        var expected = new StringBuilder()
                      .Append("USE_CODE_IF_NOT(")
                      .Append( preprocessSymbolName )
                      .Append( ')' )
                      .NewLine()
                      .Append( "message()" )
                      .NewLine()
                      .Append( "END_USE_CODE" )
                      .NewLine()
                      .ToString();

        var output = new StringBuilder();

        var ifNotDefineNode = new AstPreprocessorIfnotDefineNode
        {
            Condition = new AstSymbolExpressionNode( preprocessSymbolName )
        };

        ifNotDefineNode.Block.Statements.Add(
            new AstCallCommandExpressionNode
            {
                Left = new AstSymbolExpressionNode( "message" )
            }
        );

        var evaluator = new PreprocessEvaluator( output );
        var visitor = new MockPreprocessEvaluatorVisitor( output );

        visitor.Inject( evaluator );
        visitor.Visit( ifNotDefineNode );

        Assert.AreEqual( expected, output.ToString() );
    }
}
