using System;

using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Commands;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Domain.Ast.Analyzers.Semantics;

public class CallCommandExpressionEvaluator : ICallCommandExpressionEvaluator
{
    public CallCommandExpressionEvaluator( ICompilerMessageManger compilerMessageManger, ICommandSymbolTable commands )
    {
        throw new NotImplementedException();
    }

    public IAstNode Evaluate( IAstVisitor<IAstNode> visitor, AstCallCommandExpressionNode expr )
        => throw new NotImplementedException();
}
