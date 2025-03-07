using System.Text;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;

using NUnit.Framework;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Obfuscators;

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

        Assert.That( output.ToString(), Is.EqualTo( expected ) );
    }
}
