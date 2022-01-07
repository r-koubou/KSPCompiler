#nullable disable

using KSPCompiler.Domain.Ast.Blocks;

namespace KSPCompiler.Domain.Ast.Statements
{
    /// <summary>
    /// AST node representing an select statement
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
        public AstSelectStatement( IAstNode parent = null )
            : base( AstNodeId.SelectStatement, parent )
        {
            CaseBlocks = new AstNodeList<AstCaseBlock>( this );
        }

        #region IAstNodeAcceptor

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor )
            => visitor.Visit( this );

        ///
        /// <inheritdoc/>
        ///
        public override void AcceptChildren<T>( IAstVisitor<T> visitor )
        {
            foreach( var n in CaseBlocks )
            {
                n.Accept( visitor );
            }
        }

        #endregion IAstNodeAcceptor
    }
}
