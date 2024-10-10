using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing an integer literal
    /// </summary>
    public class AstIntLiteral : AstLiteral<int>
    {
        public override DataTypeFlag TypeFlag
            => DataTypeFlag.TypeInt;

        ///
        /// <inheritdoc/>
        ///
        public override bool IsConstant => true;

        /// <summary>
        /// Ctor
        /// </summary>
        public AstIntLiteral()
            : this( 0 ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstIntLiteral( int value )
            : this( value, NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstIntLiteral( int value, IAstNode parent )
            : base( AstNodeId.IntLiteral, parent, value ) {}

        #region IAstNodeAcceptor

        ///
        /// <inheritdoc/>
        ///
        public override int ChildNodeCount
            => 0;

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
            => visitor.Visit( this , abortTraverseToken );

        #endregion IAstNodeAcceptor
    }
}
