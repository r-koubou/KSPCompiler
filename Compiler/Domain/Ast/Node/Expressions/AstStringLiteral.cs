using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Ast.Node.Expressions
{
    /// <summary>
    /// AST node representing a string literal
    /// </summary>
    public class AstStringLiteral : AstExpressionSyntaxNode, IVariable<string>
    {
        #region IAstVariable<T>

        /// <summary>
        /// literal value
        /// </summary>
        public string Value { get; set; }

        #endregion IAstVariable<T>

        ///
        /// <inheritdoc/>
        ///
        public override bool IsConstant => true;

        /// <summary>
        /// Ctor
        /// </summary>
        public AstStringLiteral( string value = "", IAstNode? parent = null )
            : base( AstNodeId.StringLiteral, parent )
        {
            Value = value;
            TypeFlag  = DataTypeFlag.TypeString;
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
