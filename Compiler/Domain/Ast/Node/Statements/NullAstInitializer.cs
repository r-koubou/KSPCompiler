namespace KSPCompiler.Domain.Ast.Node.Statements;

public sealed class NullAstInitializer : AstInitializer
{
    public static readonly NullAstInitializer Instance = new NullAstInitializer();
    private NullAstInitializer()
        : base( AstNodeId.None, NullAstNode.Instance ) {}

    public override T Accept<T>( IAstVisitor<T> visitor )
        => visitor.Visit( this );

    public override void AcceptChildren<T>( IAstVisitor<T> visitor ) {}
}
