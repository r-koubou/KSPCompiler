using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Interactors.Analysis.Preprocessing;

using NUnit.Framework;

namespace KSPCompiler.Interactors.Tests.Analysis.Preprocess;

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
            new AstPreprocessorDefineNode( callBack, registerSymbolName )
        );

        var symbol = new AstSymbolExpressionNode
        {
            Name = evaluateSymbolName,
            TypeFlag = DataTypeFlag.TypePreprocessorSymbol
        };

        var ifdef = new AstPreprocessorIfdefineNode( callBack, symbol );
        callBack.Block.Statements.Add( ifdef);

        ast.GlobalBlocks.Add( callBack );

        analyzer.Traverse( ast );

        Assert.That( symbolTable.Count, Is.EqualTo( 1 ) );
        Assert.That( ifdef.Ignore,      Is.EqualTo( expectedIgnored ) );
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
            new AstPreprocessorDefineNode( callBack, registerSymbolName )
        );

        var symbol = new AstSymbolExpressionNode
        {
            Name     = evaluateSymbolName,
            TypeFlag = DataTypeFlag.TypePreprocessorSymbol
        };

        var ifdef = new AstPreprocessorIfnotDefineNode( callBack, symbol );
        callBack.Block.Statements.Add( ifdef);

        ast.GlobalBlocks.Add( callBack );

        analyzer.Traverse( ast );

        Assert.That( symbolTable.Count, Is.EqualTo( 1 ) );
        Assert.That( ifdef.Ignore,      Is.EqualTo( expectedIgnored ) );
    }

    [Test]
    public void UnDefinedTest()
    {
        var symbolTable = MockUtility.CreateAggregateSymbolTable().PreProcessorSymbols;
        var analyzer = new PreprocessAnalyzer( symbolTable );

        /*
        on init
            SET_CONDITION( DEMO )
            RESET_CONDITION( DEMO )
        end on
        */

        const string symbolName = "DEMO";

        var ast = new AstCompilationUnitNode();
        var callBack = new AstCallbackDeclarationNode( ast );

        callBack.Name = "init";
        callBack.Block.Statements.Add(
            new AstPreprocessorDefineNode( callBack, symbolName )
        );

        var undef = new AstPreprocessorUndefineNode( callBack, symbolName );
        callBack.Block.Statements.Add( undef);

        ast.GlobalBlocks.Add( callBack );

        analyzer.Traverse( ast );

        Assert.That( symbolTable.Count, Is.EqualTo( 0 ) );
    }

}
