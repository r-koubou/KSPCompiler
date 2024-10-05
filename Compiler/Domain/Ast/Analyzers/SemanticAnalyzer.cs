using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.CompilerMessages;

namespace KSPCompiler.Domain.Ast.Analyzers;

public partial class SemanticAnalyzer : DefaultAstVisitor, ISemanticAnalyzer
{
    private ICompilerMessageManger CompilerMessageManger { get; }

    public SemanticAnalyzer( ICompilerMessageManger compilerMessageManger )
    {
        CompilerMessageManger = compilerMessageManger;
    }

    public void Analyze( AstCompilationUnit node )
    {
        node.AcceptChildren( this );
    }
}
