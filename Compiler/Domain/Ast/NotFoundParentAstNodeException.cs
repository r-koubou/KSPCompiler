using System;

namespace KSPCompiler.Domain.Ast;

/// <summary>
/// Exception thrown if the expected node is not found when searching for the parent AST node.
/// </summary>
public sealed class NotFoundParentAstNodeException : Exception
{
    public NotFoundParentAstNodeException( Type type )
        : base( $"Parent node not found : `{type.FullName}`" ) {}
}
