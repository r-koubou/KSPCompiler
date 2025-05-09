using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;

namespace KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing an integer literal
    /// </summary>
    public class AstIntLiteralNode : AstLiteralNode<int>
    {
        public override DataTypeFlag TypeFlag
            => DataTypeFlag.TypeInt;

        ///
        /// <inheritdoc/>
        ///
        public override bool Constant => true;

        /// <summary>
        /// Ctor
        /// </summary>
        public AstIntLiteralNode()
            : this( 0 ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstIntLiteralNode( int value )
            : this( value, NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstIntLiteralNode( int value, IAstNode parent )
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
        public override IAstNode Accept( IAstVisitor visitor )
            => visitor.Visit( this );

        #endregion IAstNodeAcceptor
    }
}
