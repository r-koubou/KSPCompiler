using KSPCompiler.Commons.Text;

namespace KSPCompiler.Domain.Ast.Nodes.Statements;

public sealed class NullAstInitializerNode : AstInitializerNode
{
    public static readonly NullAstInitializerNode Instance = new NullAstInitializerNode();

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

    private NullAstInitializerNode()
        : base( AstNodeId.None, NullAstNode.Instance ) {}

    public override int ChildNodeCount
        => 0;

    public override T Accept<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
        => visitor.Visit( this , abortTraverseToken );

    public override void AcceptChildren<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken ) {}

    public override string ToString()
        => nameof( NullAstInitializerNode );
}
