using KSPCompiler.Commons.Text;

namespace KSPCompiler.Domain.Ast.Nodes;

/// <summary>
/// Null Object of <see cref="AstExpressionSyntaxNode"/>
/// </summary>
public sealed class NullAstExpressionSyntaxNode : AstExpressionSyntaxNode
{
    /// <summary>
    /// The Null Object instance.
    /// </summary>
    public static readonly NullAstExpressionSyntaxNode Instance = new();

    /// <summary>
    /// Always return <see cref="AstNodeId.None"/>.
    /// </summary>
    public override AstNodeId Id
        => AstNodeId.None;

    /// <summary>
    /// Always return zero and the set is ignored.
    /// </summary>
    public override Position Position
    {
        get => new ();
        set => _ = value;
    }

    /// <summary>
    /// Always return this instance and the set is ignored.
    /// </summary>
    public override IAstNode Parent
    {
        get => this;
        set => _ = value;
    }

    /// <summary>
    /// Always return this instance and the set is ignored.
    /// </summary>
    public override AstExpressionSyntaxNode Left
    {
        get => this;
        set => _ = value;
    }

    /// <summary>
    /// Always return this instance and the set is ignored.
    /// </summary>
    public override AstExpressionSyntaxNode Right
    {
        get => this;
        set => _ = value;
    }

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
        => nameof( NullAstExpressionSyntaxNode );
}
