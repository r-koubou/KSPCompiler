using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;

namespace KSPCompiler.Domain.Ast.Analyzers.Preprocessing;

public class PreprocessAnalyzer : DefaultAstVisitor, IAstTraversal
{
    private IKspPreProcessorSymbolTable SymbolTable { get; }

    public PreprocessAnalyzer( IKspPreProcessorSymbolTable symbolTable )
    {
        SymbolTable = symbolTable;
    }

    public void Traverse( AstCompilationUnitNode node )
    {
        node.AcceptChildren( this );
    }

    public override IAstNode Visit( AstKspPreprocessorDefineNode node )
    {
        var symbol = new KspPreProcessorSymbol
        {
            Name = node.Symbol.Name
        };

        SymbolTable.Add( symbol );

        return node;
    }

    public override IAstNode Visit( AstKspPreprocessorUndefineNode node )
    {
        var symbol = new KspPreProcessorSymbol
        {
            Name = node.Symbol.Name
        };

        SymbolTable.Remove( symbol );

        return node;
    }

    public override IAstNode Visit( AstKspPreprocessorIfdefineNode node )
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

    public override IAstNode Visit( AstKspPreprocessorIfnotDefineNode node )
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
