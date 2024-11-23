using System.Text;

using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Interactors.Analysis.Obfuscators;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

[TestFixture]
public class ArrayElementEvaluatorTest
{
    [Test]
    public void Test()
    {
        var expected = new StringBuilder()
                      .Append( "%x[1]" )
                      .ToString();

        var output = new StringBuilder();

        var expr = new AstArrayElementExpressionNode
        {
            Left = new AstSymbolExpressionNode( "%x" )
            {
                TypeFlag = DataTypeFlag.TypeIntArray
            },
            Right = new AstIntLiteralNode( 1 )
        };

        var evaluator = new ArrayElementEvaluator( output );
        var visitor = new MockArrayElementEvaluatorVisitor( output );

        visitor.Inject( evaluator );
        visitor.Visit( expr );

        ClassicAssert.AreEqual( expected, output.ToString() );
    }
}
