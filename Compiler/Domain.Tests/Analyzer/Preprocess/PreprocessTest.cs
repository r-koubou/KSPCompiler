using KSPCompiler.Domain.Ast.Analyzers;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;

using NUnit.Framework;

namespace KSPCompiler.Domain.Tests.Analyzer.Preprocess;

[TestFixture]
public class PreprocessTest
{
    [TestCase( "DEMO", "DEMO", false )]
    [TestCase( "DEMO", "DEMO2", true )]
    public void IfDefinedTest( string registerSymbolName, string evaluateSymbolName, bool expectedIgnored )
    {
        var symbolTable = MockUtility.CreateAggregateSymbolTable().PreProcessorSymbols;
        var analyzer = new PreprocessAnalyzer( symbolTable );

        /*
        on init
            SET_CONDITION( registerSymbolName )
            USE_CODE_IF( evaluateSymbolName )
            :
            : code block
            :
            END_USE_CODE
        end on
        */
        var ast = new AstCompilationUnitNode();
        var callBack = new AstCallbackDeclarationNode( ast );

        callBack.Name = "init";
        callBack.Block.Statements.Add(
            new AstKspPreprocessorDefineNode( callBack, registerSymbolName )
        );

        var symbol = new AstSymbolExpressionNode
        {
            Name = evaluateSymbolName,
            TypeFlag = DataTypeFlag.TypeKspPreprocessorSymbol
        };

        var ifdef = new AstKspPreprocessorIfdefineNode( callBack, symbol );
        callBack.Block.Statements.Add( ifdef);

        ast.GlobalBlocks.Add( callBack );

        analyzer.Traverse( ast );

        Assert.AreEqual( expectedIgnored, ifdef.Ignore );
    }

    [TestCase( "DEMO", "DEMO",  true )]
    [TestCase( "DEMO", "DEMO2", false )]
    public void IfNotDefinedTest( string registerSymbolName, string evaluateSymbolName, bool expectedIgnored )
    {
        var symbolTable = MockUtility.CreateAggregateSymbolTable().PreProcessorSymbols;
        var analyzer = new PreprocessAnalyzer( symbolTable );

        /*
        on init
            SET_CONDITION( registerSymbolName )
            USE_CODE_IF_NOT( evaluateSymbolName )
            :
            : code block
            :
            END_USE_CODE
        end on
        */
        var ast = new AstCompilationUnitNode();
        var callBack = new AstCallbackDeclarationNode( ast );

        callBack.Name = "init";
        callBack.Block.Statements.Add(
            new AstKspPreprocessorDefineNode( callBack, registerSymbolName )
        );

        var symbol = new AstSymbolExpressionNode
        {
            Name     = evaluateSymbolName,
            TypeFlag = DataTypeFlag.TypeKspPreprocessorSymbol
        };

        var ifdef = new AstKspPreprocessorIfnotDefineNode( callBack, symbol );
        callBack.Block.Statements.Add( ifdef);

        ast.GlobalBlocks.Add( callBack );

        analyzer.Traverse( ast );

        Assert.AreEqual( expectedIgnored, ifdef.Ignore );
    }
}

#region Woprk mock classes
//
#endregion

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
