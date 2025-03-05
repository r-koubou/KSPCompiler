using System.Text;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators.Extensions;
using KSPCompiler.Shared.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Ast.Nodes.Statements;

using NUnit.Framework;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Obfuscators;

[TestFixture]
public class PreprocessorTest
{
    [Test]
    public void DefineTest()
    {
        // SET_CONDITION(DEMO)

        const string preprocessSymbolName = "DEMO";
        var expected = new StringBuilder()
                      .Append("SET_CONDITION(")
                      .Append( preprocessSymbolName )
                      .Append( ')' )
                      .NewLine()
                      .ToString();

        var output = new StringBuilder();

        var defineNode = new AstPreprocessorDefineNode( preprocessSymbolName );

        var evaluator = new PreprocessEvaluator( output );
        var visitor = new MockPreprocessEvaluatorVisitor( output );

        visitor.Inject( evaluator );
        visitor.Visit( defineNode );

        Assert.That( output.ToString(), Is.EqualTo( expected ) );
    }

    [Test]
    public void UnDefineTest()
    {
        // RESET_CONDITION(DEMO)

        const string preprocessSymbolName = "DEMO";
        var expected = new StringBuilder()
                      .Append("RESET_CONDITION(")
                      .Append( preprocessSymbolName )
                      .Append( ')' )
                      .NewLine()
                      .ToString();

        var output = new StringBuilder();

        var defineNode = new AstPreprocessorUndefineNode( preprocessSymbolName );

        var evaluator = new PreprocessEvaluator( output );
        var visitor = new MockPreprocessEvaluatorVisitor( output );

        visitor.Inject( evaluator );
        visitor.Visit( defineNode );

        Assert.That( output.ToString(), Is.EqualTo( expected ) );
    }

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

        Assert.That( output.ToString(), Is.EqualTo( expected ) );
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

        Assert.That( output.ToString(), Is.EqualTo( expected ) );
    }
}
