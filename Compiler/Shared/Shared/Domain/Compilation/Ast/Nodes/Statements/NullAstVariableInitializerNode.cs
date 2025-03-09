using KSPCompiler.Shared.Text;

namespace KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Statements;

public sealed class NullAstVariableInitializerNode : AstVariableInitializerNode
{
    public static readonly NullAstVariableInitializerNode Instance = new ();

    /// <summary>
    /// Always return <see cref="AstNodeId.VariableInitializer"/>.
    /// </summary>
    public override AstNodeId Id
        => AstNodeId.VariableInitializer;

    /// <summary>
    /// Always return zero and the set is ignored.
    /// </summary>
    public override Position Position
        => Position.Zero;

    /// <summary>
    /// Always return this instance and the set is ignored.
    /// </summary>
    public override IAstNode Parent
        => this;

    /// <summary>
    /// Always return <see cref="NullAstPrimitiveInitializerNode.Instance"/>.
    /// </summary>
    public override AstPrimitiveInitializerNode PrimitiveInitializer
        => NullAstPrimitiveInitializerNode.Instance;

    /// <summary>
    /// Always return <see cref="NullAstArrayInitializerNode.Instance"/>.
    /// </summary>
    public override AstArrayInitializerNode ArrayInitializer
        => NullAstArrayInitializerNode.Instance;

    private NullAstVariableInitializerNode()
        : base( NullAstNode.Instance ) {}

    public override int ChildNodeCount
        => 0;

    public override IAstNode Accept( IAstVisitor visitor )
        => visitor.Visit( this );

    public override void AcceptChildren( IAstVisitor visitor ) {}

    public override string ToString()
        => nameof( NullAstVariableInitializerNode );
}
