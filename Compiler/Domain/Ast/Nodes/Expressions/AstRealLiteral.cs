using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing a floating-point literal
    /// </summary>
    public class AstRealLiteral : AstLiteral<double>
    {
        public override DataTypeFlag TypeFlag
            => DataTypeFlag.TypeReal;

        ///
        /// <inheritdoc/>
        ///
        public override bool IsConstant => true;

        /// <summary>
        /// Ctor
        /// </summary>
        public AstRealLiteral()
            : this( 0.0 ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstRealLiteral( double value )
            : this( value, NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstRealLiteral( double value, IAstNode parent )
            : base( AstNodeId.RealLiteral, parent, value ) {}

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
