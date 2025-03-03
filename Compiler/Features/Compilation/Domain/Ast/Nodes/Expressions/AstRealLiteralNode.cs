using KSPCompiler.Features.Compilation.Domain.Symbols.MetaData;

namespace KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing a floating-point literal
    /// </summary>
    public class AstRealLiteralNode : AstLiteralNode<double>
    {
        public override DataTypeFlag TypeFlag
            => DataTypeFlag.TypeReal;

        ///
        /// <inheritdoc/>
        ///
        public override bool Constant => true;

        /// <summary>
        /// Ctor
        /// </summary>
        public AstRealLiteralNode()
            : this( 0.0 ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstRealLiteralNode( double value )
            : this( value, NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstRealLiteralNode( double value, IAstNode parent )
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
        public override IAstNode Accept( IAstVisitor visitor )
            => visitor.Visit( this );

        #endregion IAstNodeAcceptor
    }
}
