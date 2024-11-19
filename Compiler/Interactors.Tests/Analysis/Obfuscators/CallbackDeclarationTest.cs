using System.Text;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Interactors.Analysis.Obfuscators;
using KSPCompiler.Interactors.Analysis.Obfuscators.Extensions;

using NUnit.Framework;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

[TestFixture]
public class CallbackDeclarationTest
{
    [Test]
    public void NoArgumentCallbackTest()
    {
        const string callbackName = "init";
        var expected = new StringBuilder()
                      .Append( $"on {callbackName}" )
                      .NewLine()
                      .Append( "end on" )
                      .NewLine().ToString();

        var output = new StringBuilder();
        var node = new AstCallbackDeclarationNode
        {
            Name = callbackName
        };

        var evaluator = new CallbackDeclarationEvaluator( output );
        var visitor = new MockCallbackDeclarationVisitor( output );

        visitor.Inject( evaluator );
        visitor.Visit( node );

        Assert.AreEqual( expected, output.ToString() );
    }

    [Test]
    public void ArgumentCallbackTest()
    {
        const string callbackName = "ui_control";
        const string argumentName = "$Knob";

        var expected = new StringBuilder()
                      .Append( $"on {callbackName} ({argumentName})" )
                      .NewLine()
                      .Append( "end on" )
                      .NewLine().ToString();

        var output = new StringBuilder();
        var node = new AstCallbackDeclarationNode
        {
            Name = callbackName,
            ArgumentList = new AstArgumentListNode
            {
                Arguments =
                {
                    new AstArgumentNode
                    {
                        Name = argumentName
                    }
                }
            }
        };

        var evaluator = new CallbackDeclarationEvaluator( output );
        var visitor = new MockCallbackDeclarationVisitor( output );

        visitor.Inject( evaluator );
        visitor.Visit( node );

        Assert.AreEqual( expected, output.ToString() );
    }
}
