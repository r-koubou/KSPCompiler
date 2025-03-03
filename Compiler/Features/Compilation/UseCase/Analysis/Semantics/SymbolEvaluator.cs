using KSPCompiler.Features.Compilation.Domain.Ast.Nodes;
using KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Features.Compilation.Domain.Symbols;
using KSPCompiler.Features.Compilation.Domain.Symbols.MetaData;
using KSPCompiler.Features.Compilation.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Symbols;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Extensions;
using KSPCompiler.Shared.EventEmitting;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics;

public class SymbolEvaluator : ISymbolEvaluator
{
    private IEventEmitter EventEmitter { get; }
    private AggregateSymbolTable SymbolTable { get; }

    private static AstExpressionNode CreateEvaluateNode( AstExpressionNode source, SymbolBase symbol, ISymbolDataTypeProvider symbolType )
    {
        var result = new AstSymbolExpressionNode( source.Left, symbol.Name )
        {
            Parent      = source.Parent,
            TypeFlag    = symbolType.DataType,
            Constant    = symbol.Modifier.IsConstant(),
            BuiltIn     = symbol.BuiltIn
        };

        return result;
    }

    private static AstExpressionNode CreateEvaluateNode( AstExpressionNode source, DataTypeFlag type, bool isConstant )
        => new AstSymbolExpressionNode( source.Left, source.Name )
        {
            Parent   = source.Parent,
            TypeFlag = type,
            Constant = isConstant
        };

    public SymbolEvaluator(
        IEventEmitter eventEmitter,
        AggregateSymbolTable symbolTable )
    {
        EventEmitter = eventEmitter;
        SymbolTable  = symbolTable;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstSymbolExpressionNode expr )
    {
        if( TryGetVariableSymbol( visitor, expr, out var result ) )
        {
            return result;
        }

        if( TryGetPgsSymbol( expr, out result ) )
        {
            return result;
        }

        // シンボルが見つからなかったのでエラー計上後、代替の評価結果を返す

        EventEmitter.Emit(
            expr.AsErrorEvent(
                CompilerMessageResources.semantic_error_variable_not_declared,
                expr.Name
            )
        );

        result = CreateEvaluateNode( expr, DataTypeUtility.GuessFromSymbolName( expr.Name ), expr.Constant );

        return result;
    }

    private bool TryGetVariableSymbol( IAstVisitor visitor, AstSymbolExpressionNode expr, out AstExpressionNode result )
    {
        result = NullAstExpressionNode.Instance;

        if( !SymbolTable.TrySearchVariableByName( expr.Name, out var variable ) )
        {
            return false;
        }

        // 変数への評価が確定するので参照済みフラグを立てる
        variable.State = SymbolState.Loaded;

        if( TryGetAstLiteralNode( variable, out result ) )
        {
            return true;
        }

        result = CreateEvaluateNode( expr, variable, variable );

        return true;
    }

    private bool TryGetAstLiteralNode( VariableSymbol variable, out AstExpressionNode result )
    {
        result = NullAstExpressionNode.Instance;

        if( variable.BuiltIn || !variable.Modifier.IsConstant() )
        {
            return false;
        }

        switch( variable.ConstantValue )
        {
            case int intValue:
                result = new AstIntLiteralNode( intValue );
                return true;
            case double doubleValue:
                result = new AstRealLiteralNode( doubleValue );
                return true;
            case string stringValue:
                result = new AstStringLiteralNode( stringValue );
                return true;
            default:
                return false;
        }
    }

    private bool TryGetPgsSymbol( AstSymbolExpressionNode expr, out AstExpressionNode result )
    {
        result = NullAstExpressionNode.Instance;

        // PGS key は create, set, get の呼び出し順に関係なく参照可能なので
        // シンボルテーブルを用いない
        if( !DataTypeUtility.GuessFromSymbolName( expr.Name ).IsPgsId() )
        {
            return false;
        }

        // 64 文字超の ID は使用できない仕様
        // https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/advanced-concepts#pgs
        if( expr.Name.Length > 64 )
        {
            EventEmitter.Emit(
                expr.AsErrorEvent(
                    CompilerMessageResources.semantic_error_pgs_name_maximam_length,
                    expr.Name[ ..16 ] + "..."
                )
            );

            // 上位のノードで評価を継続させるので代替のノードは生成しない
        }

        var symbol = new PgsSymbol
        {
            Name = expr.Name,
            DataType = DataTypeFlag.TypePgsId
        };

        result = CreateEvaluateNode( expr, symbol, symbol );

        return true;
    }
}
