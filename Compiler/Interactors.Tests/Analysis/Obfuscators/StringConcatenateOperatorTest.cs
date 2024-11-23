using System.Text;

using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Interactors.Analysis.Obfuscators;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

[TestFixture]
public class StringConcatenateOperatorTest
{
    [Test]
    public void Test()
    {
        var output = new StringBuilder();
        var visitor = new MockAstStringConcatenateOperatorVisitor( output );
        var evaluator = new StringConcatenateOperatorEvaluator( output );

        visitor.Inject( evaluator );

        var operatorNode = new AstStringConcatenateExpressionNode
        {
            Left  = new AstStringLiteralNode( "abc" ),
            Right = new AstStringLiteralNode( "def" )
        };

        visitor.Visit( operatorNode );
        Assert.That( output.ToString(), Is.EqualTo( "abc & def" ) );
    }
}
