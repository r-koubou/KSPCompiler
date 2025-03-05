using System.Text;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;
using KSPCompiler.Shared.Domain.Ast.Nodes.Expressions;

using NUnit.Framework;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Obfuscators;

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
