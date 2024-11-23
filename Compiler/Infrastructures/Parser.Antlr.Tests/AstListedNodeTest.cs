﻿using System.Linq;

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
            Assert.That( () =>
                {
                    list.Add( new AstAdditionExpressionNode() );
                    list.Add( new AstSubtractionExpressionNode() );
                    list.Add( new AstMultiplyingExpressionNode() );
                }, Throws.Nothing
            );

            Assert.That( list.Nodes.Count, Is.EqualTo( 3 ) );

            var count = list.Count();
            Assert.That( count, Is.EqualTo( list.Count ) );

            Assert.That( list[ 0 ], Is.InstanceOf<AstAdditionExpressionNode>() );
            Assert.That( list[ 1 ], Is.InstanceOf<AstSubtractionExpressionNode>() );
            Assert.That( list[ 2 ], Is.InstanceOf<AstMultiplyingExpressionNode>() );

        }
    }
}
