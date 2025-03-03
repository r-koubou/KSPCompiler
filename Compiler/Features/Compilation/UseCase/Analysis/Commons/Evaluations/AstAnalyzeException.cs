using System;

using KSPCompiler.Features.Compilation.Domain.Ast.Nodes;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations;

public class AstAnalyzeException : Exception
{
    public AstAnalyzeException( IAstNode node, string message )
        : base( $"{node.GetType()} - {message}") {}

    public AstAnalyzeException( object analyzerClass, IAstNode node, string message )
        : base( $"{analyzerClass.GetType()}.{node.GetType()} - {message}") {}
}
