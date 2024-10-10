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
            var list = new AstNodeList<AstExpressionNode>();
            Assert.DoesNotThrow( () => {
                list.Add( new AstAdditionExpressionNode() );
                list.Add( new AstSubtractionExpressionNode() );
                list.Add( new AstMultiplyingExpressionNode() );
            });
            Assert.IsTrue( list.Nodes.Count == 3 );

            var count = list.Count();
            Assert.IsTrue( count == list.Count );

            Assert.IsTrue( list[ 0 ] is AstAdditionExpressionNode );
            Assert.IsTrue( list[ 1 ] is AstSubtractionExpressionNode );
            Assert.IsTrue( list[ 2 ] is AstMultiplyingExpressionNode );
        }
    }
}
