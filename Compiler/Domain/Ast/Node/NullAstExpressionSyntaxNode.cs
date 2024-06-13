namespace KSPCompiler.Domain.Ast.Node;

/// <summary>
/// Null Object of <see cref="AstExpressionSyntaxNode"/>
/// </summary>
public sealed class NullAstExpressionSyntaxNode : AstExpressionSyntaxNode
{
    /// <summary>
    /// The Null Object instance.
    /// </summary>
    public static readonly NullAstExpressionSyntaxNode Instance = new();

    private NullAstExpressionSyntaxNode()
    {}

    #region IAstNodeAcceptor
    ///
    /// <inheritdoc/>
    ///
    public override T Accept<T>( IAstVisitor<T> visitor )
        => visitor.Visit( this );

    /// <summary>
    /// Do nothing.
    /// </summary>
    public override void AcceptChildren<T>( IAstVisitor<T> visitor ) {}
    #endregion IAstNodeAcceptor

    public override string ToString()
        => nameof(NullAstExpressionSyntaxNode);
}
