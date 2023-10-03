using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Ast.Node.Expressions
{
    /// <summary>
    /// AST node representing an integer literal
    /// </summary>
    public class AstIntLiteral : AstExpressionSyntaxNode, IVariable<int>
    {
        #region IAstVariable<T>

        /// <summary>
        /// literal value
        /// </summary>
        public int Value { get; set; }

        #endregion IAstVariable<T>

        ///
        /// <inheritdoc/>
        ///
        public override bool IsConstant => true;

        /// <summary>
        /// Ctor
        /// </summary>
        public AstIntLiteral( int value = 0, IAstNode? parent = null )
            : base( AstNodeId.IntLiteral, parent )
        {
            Value = value;
            TypeFlag  = DataTypeFlag.TypeInt;
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
