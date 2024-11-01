using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing a boolean literal
    /// </summary>
    public class AstBooleanLiteralNode : AstLiteralNode<bool>
    {
        public override DataTypeFlag TypeFlag
            => DataTypeFlag.TypeBool;

        ///
        /// <inheritdoc/>
        ///
        public override bool Constant => true;

        /// <summary>
        /// Ctor
        /// </summary>
        public AstBooleanLiteralNode()
            : this( false ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstBooleanLiteralNode( bool value )
            : this( value, NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstBooleanLiteralNode( bool value, IAstNode parent )
            : base( AstNodeId.BooleanLiteral, parent, value ) {}

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc/>
        ///
        public override int ChildNodeCount
            => 0;

        ///
        /// <inheritdoc/>
        ///
        public override IAstNode Accept( IAstVisitor visitor )
            => visitor.Visit( this );

        #endregion IAstNodeAcceptor
    }
}
