using KSPCompiler.Commons.Text;

namespace KSPCompiler.Domain.Ast.Nodes;

/// <summary>
/// Null Object of <see cref="AstExpressionNode"/>
/// </summary>
public sealed class NullAstExpressionNode : AstExpressionNode
{
    /// <summary>
    /// The Null Object instance.
    /// </summary>
    public static readonly NullAstExpressionNode Instance = new();

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
    public override AstExpressionNode Left
    {
        get => this;
        set => _ = value;
    }

    /// <summary>
    /// Always return this instance and the set is ignored.
    /// </summary>
    public override AstExpressionNode Right
    {
        get => this;
        set => _ = value;
    }

    private NullAstExpressionNode()
    {}

    #region IAstNodeAcceptor

    ///
    /// <inheritdoc />
    ///
    public override int ChildNodeCount
        => 2;

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
        => nameof( NullAstExpressionNode );
}
