using System.Text;

using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Interactors.Analysis.Obfuscators;

using NUnit.Framework;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

[TestFixture]
public class ArrayElementEvaluatorTest
{
    [Test]
    public void Test()
    {
        const string expected = "[1]";

        var output = new StringBuilder();

        var expr = new AstArrayElementExpressionNode
        {
            Left = new AstIntLiteralNode( 1 )
        };

        var evaluator = new ArrayElementEvaluator( output );
        var visitor = new MockArrayElementEvaluatorVisitor( output );

        visitor.Inject( evaluator );
        visitor.Visit( expr );

        Assert.AreEqual( expected, output.ToString() );
    }
}
