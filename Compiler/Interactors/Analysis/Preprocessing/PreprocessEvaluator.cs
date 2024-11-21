using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Interactors.Analysis.Commons.Evaluations;
using KSPCompiler.UseCases.Analysis.Evaluations.Preprocessing;

namespace KSPCompiler.Interactors.Analysis.Preprocessing;

public class PreprocessEvaluator : IPreprocessEvaluator
{
    private IPreProcessorSymbolTable SymbolTable { get; }

    public PreprocessEvaluator( IPreProcessorSymbolTable symbolTable )
    {
        SymbolTable = symbolTable;
    }

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
            throw new AstAnalyzeException( node, $"Symbol is not a preprocessor symbol (={symbolNode.TypeFlag})" );
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
            throw new AstAnalyzeException( node, $"Symbol is not a preprocessor symbol (={symbolNode.TypeFlag})" );
        }

        // シンボルが「見つかれば」コードブロック自体を無視しても良いマークを行う
        // この時点でコードブロックのノードは維持する
        node.Ignore = SymbolTable.TrySearchByName( symbolNode.Name, out _ );

        return node;
    }
}
