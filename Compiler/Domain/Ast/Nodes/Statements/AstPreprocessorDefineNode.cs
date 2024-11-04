using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Ast.Nodes.Statements
{
    /// <summary>
    /// AST node representing a KSP Preprocessor: SET_CONDITION
    /// </summary>
    public class AstPreprocessorDefineNode : AstStatementNode
    {
        /// <summary>
        /// preprocessor symbol
        /// </summary>
        public AstExpressionNode Symbol { get; }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstPreprocessorDefineNode()
            : this( NullAstNode.Instance, string.Empty ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstPreprocessorDefineNode( string name )
            : this( NullAstNode.Instance, name ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstPreprocessorDefineNode( IAstNode parent, string name )
            : base( AstNodeId.PreprocessorDefine, parent )
        {
            Symbol = new AstSymbolExpressionNode
            {
                Parent   = this,
                TypeFlag = DataTypeFlag.TypePreprocessorSymbol,
                Name     = name
            };
        }

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc />
        ///
        public override int ChildNodeCount
            => 1;

        ///
        /// <inheritdoc/>
        ///
        public override IAstNode Accept( IAstVisitor visitor )
            => visitor.Visit( this );

        ///
        /// <inheritdoc/>
        ///
        public override void AcceptChildren( IAstVisitor visitor )
        {
            Symbol.Accept( visitor );
        }

        #endregion IAstNodeAcceptor
    }
}
