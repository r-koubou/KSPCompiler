using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing a string literal
    /// </summary>
    public class AstStringLiteral : AstSymbolExpression, IVariable<string>
    {
        #region IAstVariable<T>

        /// <summary>
        /// literal value
        /// </summary>
        public string Value { get; set; }

        #endregion IAstVariable<T>

        public override DataTypeFlag TypeFlag
            => DataTypeFlag.TypeString;

        ///
        /// <inheritdoc/>
        ///
        public override bool IsConstant => true;

        /// <summary>
        /// Ctor
        /// </summary>
        public AstStringLiteral()
            : this( "" ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstStringLiteral( string value )
            : this( value, NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstStringLiteral( string value, IAstNode parent )
            : base( AstNodeId.StringLiteral, parent )
        {
            Value = value;
        }

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc/>
        ///
        public override int ChildNodeCount
            => 0;

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
            => visitor.Visit( this , abortTraverseToken );

        #endregion IAstNodeAcceptor
    }
}
