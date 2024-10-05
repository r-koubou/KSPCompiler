using KSPCompiler.Domain.Ast.Nodes.Blocks;

namespace KSPCompiler.Domain.Ast.Nodes.Statements
{
    /// <summary>
    /// AST node representing a select statement
    /// </summary>
    public class AstSelectStatement : AstStatementSyntaxNode
    {
        /// <summary>
        /// condition
        /// </summary>
        public AstExpressionSyntaxNode Condition { get; set; }

        /// <summary>
        /// case list
        /// </summary>
        public AstNodeList<AstCaseBlock> CaseBlocks { get; }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstSelectStatement()
            : this( NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstSelectStatement( IAstNode parent )
            : base( AstNodeId.SelectStatement, parent )
        {
            Condition  = NullAstExpressionSyntaxNode.Instance;
            CaseBlocks = new AstNodeList<AstCaseBlock>( this );
        }

        #region IAstNodeAcceptor

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
            => visitor.Visit( this , abortTraverseToken );

        ///
        /// <inheritdoc/>
        ///
        public override void AcceptChildren<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
        {
            if( abortTraverseToken.Aborted )
            {
                return;
            }

            foreach( var n in CaseBlocks )
            {
                n.Accept( visitor, abortTraverseToken );

                if( abortTraverseToken.Aborted )
                {
                    return;
                }
            }
        }

        #endregion IAstNodeAcceptor
    }
}
