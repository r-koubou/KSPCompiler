﻿using System.Collections.Generic;

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Extensions;
using KSPCompiler.Infrastructures.Parser.Antlr.Translators.Extensions;

// ReSharper disable UnusedMember.Local

namespace KSPCompiler.Infrastructures.Parser.Antlr.Translators
{
    /// <summary>
    /// Implementation of AST generation process based on CST.
    /// </summary>
    public partial class CstConverterVisitor : KSPParserBaseVisitor<AstNode>
    {
        private void SetupChildNode(
            IAstNode parent,
            IAstNode child,
            ParserRuleContext? childContext )
        {
            if( child.IsNull() || child.Id == AstNodeId.None )
            {
                return;
            }

            if( childContext != null )
            {
                child.Import( childContext );
            }

            child.Parent = parent!;
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
