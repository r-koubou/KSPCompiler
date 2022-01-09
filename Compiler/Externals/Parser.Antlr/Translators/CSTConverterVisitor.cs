using System.Collections.Generic;

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

using KSPCompiler.Domain.Ast;
using KSPCompiler.Externals.Parser.Antlr.Translators.Extensions;

// ReSharper disable UnusedMember.Local

namespace KSPCompiler.Externals.Parser.Antlr.Translators
{
    /// <summary>
    /// Implementation of AST generation process based on CST.
    /// </summary>
    public partial class CSTConverterVisitor : KSPParserBaseVisitor<AstNode>
    {
        private void SetupChildNode(
            IAstNode? parent,
            IAstNode? child,
            ParserRuleContext? childContext )
        {
            if( IAstNode.IsNone( child ) )
            {
                return;
            }

            if( childContext != null )
            {
                child?.Import( childContext );
            }

            if( child != null )
            {
                child.Parent = parent!;
            }
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
            foreach( var n in children )
            {
                var child = n.Accept( this );
                if( child != null )
                {
                    destList.Add( child );
                }
            }
            return dest;
        }
    }
}
