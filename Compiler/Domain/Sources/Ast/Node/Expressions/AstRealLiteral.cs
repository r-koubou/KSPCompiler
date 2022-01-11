using KSPCompiler.Domain.Ast.Symbols;

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
        public AstRealLiteral( double value = 0, IAstNode? parent = null )
            : base( AstNodeId.IntLiteral, parent )
        {
            Value = value;
            Type  = DataType.Real;
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
