using KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Features.Compilation.Domain.Symbols.MetaData;

namespace KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Statements
{
    /// <summary>
    /// AST node representing a KSP Preprocessor: RESET_CONDITION
    /// </summary>
    public class AstPreprocessorUndefineNode : AstStatementNode
    {
        /// <summary>
        /// preprocessor symbol
        /// </summary>
        public AstExpressionNode Symbol { get; }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstPreprocessorUndefineNode()
            : this( NullAstNode.Instance, string.Empty ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstPreprocessorUndefineNode( string name )
            : this( NullAstNode.Instance, name ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstPreprocessorUndefineNode( IAstNode parent, string name )
            : base( AstNodeId.PreprocessorUndefine, parent )
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
