using System;

using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Ast.Analyzers;

public class AstAnalyzeException : Exception
{
    public AstAnalyzeException( IAstNode node, string message )
        : base( $"{node.GetType()} - {message}") {}

    public AstAnalyzeException( object analyzerClass, IAstNode node, string message )
        : base( $"{analyzerClass.GetType()}.{node.GetType()} - {message}") {}
}
