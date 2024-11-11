using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.Extensions;
using KSPCompiler.Interactors.Analysis.Commons.Extensions;
using KSPCompiler.Resources;
using KSPCompiler.UseCases.Analysis.Evaluations.Declarations;

namespace KSPCompiler.Interactors.Analysis.Semantics;

public class CallbackDeclarationEvaluator : ICallbackDeclarationEvaluator
{
    private ICompilerMessageManger CompilerMessageManger { get; }
    private ISymbolTable<CallbackSymbol> BuiltInCallbackSymbols { get; }
    private ISymbolTable<CallbackSymbol> UserCallbackSymbols { get; }

    public CallbackDeclarationEvaluator(
        ICompilerMessageManger compilerMessageManger,
        ISymbolTable<CallbackSymbol> builtInCallbackSymbols,
        ISymbolTable<CallbackSymbol> userCallbackSymbols )
    {
        CompilerMessageManger  = compilerMessageManger;
        BuiltInCallbackSymbols = builtInCallbackSymbols;
        UserCallbackSymbols    = userCallbackSymbols;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstCallbackDeclarationNode node )
    {
#if false
        if( node.ArgumentList.HasArgument )
        {
            // コールバック引数リストあり
            // 現状、コールバック引数は　on init で宣言した変数
            foreach( var arg in node.ArgumentList.Arguments )
            {
                if( !VariableSymbolTable.TrySearchByName( arg.Name, out var variable ) )
                {
                    // on init で未定義
                    CompilerMessageManger.Error( node, CompilerMessageResources.symbol_error_declare_variable_unkown, arg.Name );
                }
                else
                {
                    variable.Referenced = true;
                }
            }
        }
#endif

        CallbackSymbol thisCallback;

        // NI予約済みコールバックの検査
        if( !BuiltInCallbackSymbols.TrySearchByName( node.Name, out var builtInCallback ) )
        {
            CompilerMessageManger.Warning(
                node,
                CompilerMessageResources.symbol_warning_declare_callback_unkown,
                node.Name
            );

            // 暫定のシンボル生成
            thisCallback = node.As();
        }
        else
        {
            thisCallback = builtInCallback;
        }

        if( !UserCallbackSymbols.Add( thisCallback ) )
        {
            CompilerMessageManger.Error(
                node,
                CompilerMessageResources.symbol_error_declare_callback_already,
                node.Name
            );
        }

        return node;
    }
}
