using KSPCompiler.Domain.Ast.Node;
using KSPCompiler.Domain.Ast.Node.Blocks;
using KSPCompiler.Domain.Ast.Node.Statements;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Domain.Ast.Analyzers;

public class SymbolCollector : DefaultAstVisitor, ISymbolCollector
{
    private const string InitCallbackName = "init";

    private ICompilerMessageManger CompilerMessageManger { get; }

    #region Symbol Tables
    public ISymbolTable<VariableSymbol> Variables { get; } = new VariableSymbolTable();
    #endregion

    public SymbolCollector( ICompilerMessageManger compilerMessageManger )
    {
        CompilerMessageManger = compilerMessageManger;
    }

    public void Analyze( AstCompilationUnit node )
    {
        node.Accept( this );
    }

    #region IAstVisitor

    public override IAstNode Visit( AstVariableDeclaration node )
    {
        return node;
    }

    #endregion
}
