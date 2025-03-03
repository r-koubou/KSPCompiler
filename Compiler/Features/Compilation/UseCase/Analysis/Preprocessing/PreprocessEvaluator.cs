using KSPCompiler.Features.Compilation.Domain.Ast.Nodes;
using KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Statements;
using KSPCompiler.Features.Compilation.Domain.Symbols;
using KSPCompiler.Features.Compilation.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Preprocessing;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Extensions;
using KSPCompiler.Shared.EventEmitting;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Preprocessing;

public class PreprocessEvaluator( IPreProcessorSymbolTable symbolTable, IEventEmitter eventEmitter ) : IPreprocessEvaluator
{
    private IPreProcessorSymbolTable SymbolTable { get; } = symbolTable;

    public IAstNode Evaluate( IAstVisitor visitor, AstPreprocessorDefineNode node )
    {
        var symbol = new PreProcessorSymbol
        {
            Name = node.Symbol.Name
        };

        SymbolTable.Add( symbol );

        return node;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstPreprocessorUndefineNode node )
    {
        var symbol = new PreProcessorSymbol
        {
            Name = node.Symbol.Name
        };

        SymbolTable.Remove( symbol );

        return node;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstPreprocessorIfdefineNode node )
    {
        if( node.Condition is not AstSymbolExpressionNode symbolNode )
        {
            throw new AstAnalyzeException( node, $"Currently only {nameof( AstSymbolExpressionNode )} is supported." );
        }

        if( !symbolNode.TypeFlag.IsPreprocessor() )
        {
            eventEmitter.Emit(
                node.AsErrorEvent(
                    CompilerMessageResources.preprocess_error_symbol_incompatible,
                    symbolNode.TypeFlag
                )
            );

            return node;
        }

        // シンボルが「見つからなければ」コードブロック自体を無視しても良いマークを行う
        // この時点でコードブロックのノードは維持する
        node.Ignore = !SymbolTable.TrySearchByName( symbolNode.Name, out _ );

        return node;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstPreprocessorIfnotDefineNode node )
    {
        if( node.Condition is not AstSymbolExpressionNode symbolNode )
        {
            throw new AstAnalyzeException( node, $"Currently only {nameof( AstSymbolExpressionNode )} is supported." );
        }

        if( !symbolNode.TypeFlag.IsPreprocessor() )
        {
            eventEmitter.Emit(
                node.AsErrorEvent(
                    CompilerMessageResources.preprocess_error_symbol_incompatible,
                    symbolNode.TypeFlag
                )
            );

            return node;
        }

        // シンボルが「見つかれば」コードブロック自体を無視しても良いマークを行う
        // この時点でコードブロックのノードは維持する
        node.Ignore = SymbolTable.TrySearchByName( symbolNode.Name, out _ );

        return node;
    }
}
