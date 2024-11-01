using System;

using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Ast.Nodes.Statements
{
    /// <summary>
    /// AST node representing a KSP Preprocessor: SET_CONDITION
    /// </summary>
    public class AstKspPreprocessorDefineNode : AstStatementNode
    {
        /// <summary>
        /// preprocessor symbol
        /// </summary>
        public AstExpressionNode Symbol { get; }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstKspPreprocessorDefineNode()
            : this( NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstKspPreprocessorDefineNode( IAstNode parent )
            : base( AstNodeId.KspPreprocessorDefine, parent )
        {
            Symbol = new AstSymbolExpressionNode
            {
                Parent   = this,
                TypeFlag = DataTypeFlag.TypeKspPreprocessorSymbol
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
            // Do nothing
        }

        #endregion IAstNodeAcceptor
    }
}
