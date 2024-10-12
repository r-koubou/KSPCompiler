using System;

using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Operators;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Domain.Ast.Analyzers.Semantics;

public sealed class AssignOperatorEvaluator : IAssignOperatorEvaluator
{
    private IAstVisitor AstVisitor { get; }
    private ICompilerMessageManger CompilerMessageManger { get; }
    private ISymbolTable<VariableSymbol> VariableSymbolTable { get; }

    public AssignOperatorEvaluator(
        IAstVisitor astVisitor,
        ICompilerMessageManger compilerMessageManger,
        ISymbolTable<VariableSymbol> variableSymbolTable )
    {
        AstVisitor            = astVisitor;
        CompilerMessageManger = compilerMessageManger;
        VariableSymbolTable   = variableSymbolTable;
    }

    public IAstNode Evaluate( IAstVisitor<IAstNode> visitor, AstExpressionNode expr, AbortTraverseToken abortTraverseToken )
    {
        throw new NotImplementedException();
    }
}
