namespace KSPCompiler.Domain.Ast.Blocks
{
    /// <summary>
    /// AST node representing a callback definition
    /// </summary>
    public partial class AstCallbackDeclaration : AstFunctionalSyntaxNode
    {
        /// <summary>
        /// Argument node list
        /// </summary>
        public AstArgumentList ArgumentList { get; private set; }

        /// <summary>
        /// Whether one or more arguments are stored in the ArgumentList or not.
        /// </summary>
        public virtual bool HasArgument => ArgumentList.HasArgument;

        /// <summary>
        /// Number of arguments stored in ArgumentList.
        /// </summary>
        public virtual int ArgumentCount => HasArgument ? ArgumentList.Arguments.Count : 0;

        private partial void Initialize()
        {
            ArgumentList = new AstArgumentList( this );
        }

    }
}