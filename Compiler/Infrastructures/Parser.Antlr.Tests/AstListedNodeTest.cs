using System.Linq;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

using NUnit.Framework;

namespace KSPCompiler.Parser.Antlr.Tests
{
    public class AstListedNodeTest
    {
        [Test]
        public void ListTest()
        {
            var list = new AstNodeList<AstExpressionSyntaxNode>();
            Assert.DoesNotThrow( () => {
                list.Add( new AstAdditionExpression() );
                list.Add( new AstSubtractionExpression() );
                list.Add( new AstMultiplyingExpression() );
            });
            Assert.IsTrue( list.Nodes.Count == 3 );

            var count = list.Count();
            Assert.IsTrue( count == list.Count );

            Assert.IsTrue( list[ 0 ] is AstAdditionExpression );
            Assert.IsTrue( list[ 1 ] is AstSubtractionExpression );
            Assert.IsTrue( list[ 2 ] is AstMultiplyingExpression );
        }
    }
}
