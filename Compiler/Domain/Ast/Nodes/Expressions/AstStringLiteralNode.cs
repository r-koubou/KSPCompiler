using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing a string literal
    /// </summary>
    public class AstStringLiteralNode : AstLiteralNode<string>
    {
        public override DataTypeFlag TypeFlag
            => DataTypeFlag.TypeString;

        ///
        /// <inheritdoc/>
        ///
        public override bool Constant => true;

        /// <summary>
        /// Ctor
        /// </summary>
        public AstStringLiteralNode()
            : this( "" ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstStringLiteralNode( string value )
            : this( value, NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstStringLiteralNode( string value, IAstNode parent )
            : base( AstNodeId.StringLiteral, parent, value ) {}

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc/>
        ///
        public override int ChildNodeCount
            => 0;

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor )
            => visitor.Visit( this );

        #endregion IAstNodeAcceptor
    }
}
