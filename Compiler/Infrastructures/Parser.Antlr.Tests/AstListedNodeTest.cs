using System.Linq;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace KSPCompiler.Parser.Antlr.Tests
{
    public class AstListedNodeTest
    {
        [Test]
        public void ListTest()
        {
            var list = new AstNodeList<AstExpressionNode>();
            ClassicAssert.DoesNotThrow( () => {
                list.Add( new AstAdditionExpressionNode() );
                list.Add( new AstSubtractionExpressionNode() );
                list.Add( new AstMultiplyingExpressionNode() );
            });
            ClassicAssert.IsTrue( list.Nodes.Count == 3 );

            var count = list.Count();
            ClassicAssert.IsTrue( count == list.Count );

            ClassicAssert.IsTrue( list[ 0 ] is AstAdditionExpressionNode );
            ClassicAssert.IsTrue( list[ 1 ] is AstSubtractionExpressionNode );
            ClassicAssert.IsTrue( list[ 2 ] is AstMultiplyingExpressionNode );
        }
    }
}
