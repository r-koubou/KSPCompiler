using System.Collections.Generic;
using System.Linq;

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

using KSPCompiler.Domain.Ast;
using KSPCompiler.Externals.Antlr.Generated;

namespace KSPCompiler.Externals.Antlr.Cst
{
    /// <summary>
    /// Implementation of AST generation process based on CST.
    /// </summary>
    internal partial class CstConverterVisitor : KSPParserBaseVisitor<AstNode>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public CstConverterVisitor() {}

        private void SetupChildNode(
            IAstNode? parent,
            IAstNode? child,
            ParserRuleContext childContext )
        {
            if( IAstNode.IsNone( child ) )
            {
                return;
            }

            child.Import( childContext );
            child.Parent = parent;
        }

        /// <summary>
        /// Create an AST of a child node and store it in the specified <see cref="AstNodeList{TNode}"/> list.
        /// </summary>
        /// <returns>dest</returns>
        private AstNode SetupChildrenNode(
            AstNode dest,
            AstNodeList<AstNode> destList,
            IEnumerable<IParseTree> children )
        {
            foreach( var x
                in children
                  .Select( n => n.Accept( this ) )
                  .Where( child => child != null ) )
            {
                destList.Add( x );
            }

            return dest;
        }
    }
}
