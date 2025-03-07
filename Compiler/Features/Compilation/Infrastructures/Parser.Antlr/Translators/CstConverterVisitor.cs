using System.Collections.Generic;

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

using KSPCompiler.Features.Compilation.Infrastructures.Parser.Antlr.Translators.Extensions;
using KSPCompiler.Infrastructures.Parser.Antlr;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Extensions;
using KSPCompiler.Shared.EventEmitting;

// ReSharper disable UnusedMember.Local

namespace KSPCompiler.Features.Compilation.Infrastructures.Parser.Antlr.Translators
{
    /// <summary>
    /// Implementation of AST generation process based on CST.
    /// </summary>
    public partial class CstConverterVisitor : KSPParserBaseVisitor<AstNode>
    {
        private readonly BufferedTokenStream tokenStream;
        private readonly IEventEmitter eventEmitter;

        public CstConverterVisitor( BufferedTokenStream tokenStream, IEventEmitter eventEmitter )
        {
            this.tokenStream  = tokenStream;
            this.eventEmitter = eventEmitter;
        }

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
                child.Import( tokenStream, childContext );
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

        private AstNode VisitCallCommand( ParserRuleContext context, ParserRuleContext callExpression, ParserRuleContext callArguments )
        {
            var node = VisitExpressionNodeImpl<AstCallCommandExpressionNode>( context, callExpression, callArguments );
            node.Import( tokenStream, context );

            return node;
        }
    }
}
