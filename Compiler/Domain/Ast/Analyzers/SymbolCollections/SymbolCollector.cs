using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Declarations;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Domain.Ast.Analyzers.SymbolCollections;

// TODO ビルトインなど予約済みのシンボルを事前にファイルからロードする（変数、コールバック、コマンド）

public sealed class SymbolCollector : DefaultAstVisitor, IAstTraversal
{
    private IVariableDeclarationEvaluator VariableDeclarationEvaluator { get; }
    private ICallbackDeclarationEvaluator CallbackDeclarationEvaluator { get; }
    private IUserFunctionDeclarationEvaluator UserFunctionDeclarationEvaluator { get; }

    public SymbolCollector( ICompilerMessageManger compilerMessageManger, AggregateSymbolTable symbolTable )
    {
        VariableDeclarationEvaluator     = new VariableDeclarationEvaluator( compilerMessageManger, symbolTable.Variables, symbolTable.UITypes );
        CallbackDeclarationEvaluator     = new CallbackDeclarationEvaluator( compilerMessageManger, symbolTable.ReservedCallbacks, symbolTable.UserCallbacks );
        UserFunctionDeclarationEvaluator = new UserFunctionDeclarationEvaluator( compilerMessageManger, symbolTable.UserFunctions );
    }

    public void Traverse( AstCompilationUnitNode node )
    {
        node.Accept( this );
    }

    public override IAstNode Visit( AstVariableDeclarationNode node )
        => VariableDeclarationEvaluator.Evaluate( node );

    public override IAstNode Visit( AstCallbackDeclarationNode node )
    {
        // コールバック宣言の評価
        CallbackDeclarationEvaluator.Evaluate( node );

        // コールバック内の変数宣言の評価
        node.AcceptChildren( this );

        return node;
    }

    public override IAstNode Visit( AstUserFunctionDeclarationNode node )
        => UserFunctionDeclarationEvaluator.Evaluate( node );
}
