using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;

namespace KSPCompiler.Domain.Ast.Analyzers.Preprocessing;

public class PreprocessAnalyzer : DefaultAstVisitor, IAstTraversal
{
    private IPreProcessorSymbolTable SymbolTable { get; }

    public PreprocessAnalyzer( IPreProcessorSymbolTable symbolTable )
    {
        SymbolTable = symbolTable;
    }

    public void Traverse( AstCompilationUnitNode node )
    {
        node.AcceptChildren( this );
    }

    public override IAstNode Visit( AstPreprocessorDefineNode node )
    {
        var symbol = new PreProcessorSymbol
        {
            Name = node.Symbol.Name
        };

        SymbolTable.Add( symbol );

        return node;
    }

    public override IAstNode Visit( AstPreprocessorUndefineNode node )
    {
        var symbol = new PreProcessorSymbol
        {
            Name = node.Symbol.Name
        };

        SymbolTable.Remove( symbol );

        return node;
    }

    public override IAstNode Visit( AstPreprocessorIfdefineNode node )
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

    public override IAstNode Visit( AstPreprocessorIfnotDefineNode node )
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
