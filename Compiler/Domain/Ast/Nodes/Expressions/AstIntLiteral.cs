using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing an integer literal
    /// </summary>
    public class AstIntLiteral : AstSymbolExpression, IVariable<int>
    {
        #region IAstVariable<T>

        /// <summary>
        /// literal value
        /// </summary>
        public int Value { get; set; }

        #endregion IAstVariable<T>

        public override DataTypeFlag TypeFlag
            => DataTypeFlag.TypeInt;

        ///
        /// <inheritdoc/>
        ///
        public override bool IsConstant => true;

        /// <summary>
        /// Ctor
        /// </summary>
        public AstIntLiteral()
            : this( 0 ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstIntLiteral( int value )
            : this( value, NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstIntLiteral( int value, IAstNode parent )
            : base( AstNodeId.IntLiteral, parent )
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
