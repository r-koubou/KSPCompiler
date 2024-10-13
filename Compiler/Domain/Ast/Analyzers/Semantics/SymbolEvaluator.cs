using System;

using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Symbols;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Domain.Ast.Analyzers.Semantics;

public class SymbolEvaluator : ISymbolEvaluator
{
    private IAstVisitor<IAstNode> Visitor { get; }
    private ICompilerMessageManger CompilerMessageManger { get; }
    private ISymbolTable<VariableSymbol> VariableSymbolTable { get; }

    public SymbolEvaluator( IAstVisitor<IAstNode> visitor, ICompilerMessageManger compilerMessageManger, ISymbolTable<VariableSymbol> variableSymbolTable )
    {
        CompilerMessageManger = compilerMessageManger;
        Visitor               = visitor;
        VariableSymbolTable   = variableSymbolTable;
    }

    public IAstNode Evaluate( IAstVisitor<IAstNode> visitor, AstSymbolExpressionNode expr, AbortTraverseToken abortTraverseToken )
    {
        throw new NotImplementedException();
    }
}
