using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing a floating-point literal
    /// </summary>
    public class AstRealLiteral : AstSymbolExpression, IVariable<double>
    {
        #region IAstVariable<T>

        /// <summary>
        /// literal value
        /// </summary>
        public double Value { get; set; }

        #endregion IAstVariable<T>

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
            : base( AstNodeId.RealLiteral, parent )
        {
            Value = value;
        }

        #region IAstNodeAcceptor

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
            => visitor.Visit( this , abortTraverseToken );

        #endregion IAstNodeAcceptor
    }
}
