using KSPCompiler.Commons.Text;

namespace KSPCompiler.Domain.Ast.Nodes.Statements;

public sealed class NullAstInitializer : AstInitializer
{
    public static readonly NullAstInitializer Instance = new NullAstInitializer();

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
        get => new();
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

    private NullAstInitializer()
        : base( AstNodeId.None, NullAstNode.Instance ) {}

    public override T Accept<T>( IAstVisitor<T> visitor )
        => visitor.Visit( this );

    public override void AcceptChildren<T>( IAstVisitor<T> visitor ) {}

    public override string ToString()
        => nameof( NullAstInitializer );
}
