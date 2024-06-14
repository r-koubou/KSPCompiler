using KSPCompiler.Domain.Ast.Node;
using KSPCompiler.Domain.Ast.Node.Blocks;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Domain.Ast.Analyzers;

public class SymbolAnalyzer : DefaultAstVisitor, ISymbolAnalyzer
{
    private ICompilerMessageManger CompilerMessageManger { get; }

    #region Symbol Tables
    public ISymbolTable<VariableSymbol> Variables { get; } = new VariableSymbolTable();
    #endregion

    public SymbolAnalyzer( ICompilerMessageManger compilerMessageManger )
    {
        CompilerMessageManger = compilerMessageManger;
    }

    public void Analyze( AstCompilationUnit node )
    {
        node.Accept( this );
    }

    #region IAstVisitor
    public override IAstNode Visit( AstCallbackDeclaration node )
    {
        return node.Name == "init"
            // init コールバックでのみ変数宣言が可能
            ? AnalyzeInitCallback( node )
            // init 以外のコールバックでは変数宣言はできない
            // 変数宣言を見つけ次第エラー
            : AnalyzeNonInitCallback( node );
    }

    private IAstNode AnalyzeNonInitCallback( AstCallbackDeclaration node )
    {
        // TODO 解析、変数宣言を見つけたらエラー扱いにする
        return VisitChildren( node );
    }

    private IAstNode AnalyzeInitCallback( AstCallbackDeclaration node )
    {
        // TODO 解析、変数宣言を見つけたら変数テーブルへ追加
        return VisitChildren( node );
    }

    #endregion
}
