using KSPCompiler.Commons.Text;

namespace KSPCompiler.Domain.Ast.Nodes.Statements;

public sealed class NullAstInitializerNode : AstVariableInitializerNode
{
    public static readonly NullAstInitializerNode Instance = new NullAstInitializerNode();

    /// <summary>
    /// Always return <see cref="AstNodeId.VariableInitializer"/>.
    /// </summary>
    public override AstNodeId Id
        => AstNodeId.VariableInitializer;

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

    private NullAstInitializerNode()
        : base( NullAstNode.Instance ) {}

    public override int ChildNodeCount
        => 0;

    public override T Accept<T>( IAstVisitor<T> visitor )
        => visitor.Visit( this );

    public override void AcceptChildren<T>( IAstVisitor<T> visitor ) {}

    public override string ToString()
        => nameof( NullAstInitializerNode );
}
