namespace {{ namespace }};

/// <summary>
/// AST node representing {{description}}
/// </summary>
public class {{ class_name }}Node : AstFunctionalNode
{
    /// <summary>
    /// Ctor
    /// </summary>
    public {{ class_name }}Node() : this( NullAstNode.Instance ) {}

    /// <summary>
    /// Ctor
    /// </summary>
    public {{ class_name }}Node( IAstNode parent )
        : base( AstNodeId.{{name}}, parent )
    {
    }

    #region IAstNodeAcceptor

    ///
    /// <inheritdoc/>
    ///
    public override IAstNode Accept( IAstVisitor visitor )
        => visitor.Visit( this );

    #endregion IAstNodeAcceptor
}
