using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Ast.Node.Expressions
{
    /// <summary>
    /// AST node representing a floating-point literal
    /// </summary>
    public class AstRealLiteral : AstExpressionSyntaxNode, IVariable<double>
    {
        #region IAstVariable<T>

        /// <summary>
        /// literal value
        /// </summary>
        public double Value { get; set; }

        #endregion IAstVariable<T>

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
            : base( AstNodeId.RealLiteral )
        {
            Value = value;
            TypeFlag  = DataTypeFlag.TypeReal;
        }

        #region IAstNodeAcceptor

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor )
            => visitor.Visit( this );

        #endregion IAstNodeAcceptor
    }
}
