using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Domain.Ast.Analyzers;

public partial class SemanticAnalyzer : DefaultAstVisitor, ISemanticAnalyzer
{
    private ICompilerMessageManger CompilerMessageManger { get; }

    private ISymbolTable<VariableSymbol> VariableSymbolTable { get; }

    public SemanticAnalyzer(
        ICompilerMessageManger compilerMessageManger,
        ISymbolTable<VariableSymbol> variableSymbolTable )
    {
        CompilerMessageManger = compilerMessageManger;
        VariableSymbolTable   = variableSymbolTable;
    }

    public void Analyze( AstCompilationUnit node, AbortTraverseToken abortTraverseToken)
    {
        node.AcceptChildren( this, abortTraverseToken );
    }
}
