using KSPCompiler.Commons.Text;

namespace KSPCompiler.Domain.Ast.Nodes.Statements;

public sealed class NullAstPrimitiveInitializerNode : AstPrimitiveInitializerNode
{
    public static readonly AstPrimitiveInitializerNode Instance = new NullAstPrimitiveInitializerNode();

    /// <summary>
    /// Always return <see cref="AstNodeId.PrimitiveInitializer"/>.
    /// </summary>
    public override AstNodeId Id
        => AstNodeId.PrimitiveInitializer;

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
}
