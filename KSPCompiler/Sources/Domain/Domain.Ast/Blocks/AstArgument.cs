using System;

namespace KSPCompiler.Domain.Ast.Blocks
{
    public partial class AstArgument
    {
        private partial void Initialize()
        {}

        #region INameable
        public string Name { get; set; } = string.Empty;
        #endregion

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc/>
        ///
        public override void AcceptChildren<T>( IAstVisitor<T> visitor )
        {
            // Do nothing
        }
        #endregion IAstNodeAcceptor

    }
}