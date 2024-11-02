using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public class MockAstStatementNode : AstStatementNode
{
    public MockAstStatementNode() : base( AstNodeId.None, NullAstNode.Instance ) {}

    public MockAstStatementNode( IAstNode parent ) : base( AstNodeId.None, parent ) {}

    public override int ChildNodeCount
        => 0;

    public override IAstNode Accept( IAstVisitor visitor )
        => NullAstNode.Instance;

    public override void AcceptChildren( IAstVisitor visitor ) {}
}
