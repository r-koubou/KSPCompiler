using System;
using System.IO;

using KSPCompiler.Domain.Ast.Analyzers;
using KSPCompiler.Domain.Ast.Analyzers.SymbolCollections;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Parser.Antlr.Tests.Commons;

using NUnit.Framework;

namespace KSPCompiler.Parser.Antlr.Tests;

[TestFixture]
public class AntlrSymbolCollectionTest
{
    private static readonly string TestDataDirectory = Path.Combine( "TestData", "SymbolAnalyzeTest" );

    private static AggregateSymbolTable CreateAggregateSymbolTable()
        => new (
            new VariableSymbolTable(),
            new UITypeSymbolTable(),
            new CommandSymbolTable(),
            new CallbackSymbolTable(),
            new CallbackSymbolTable(),
            new UserFunctionSymbolTable(),
            new KspPreProcessorSymbolTable(),
            new PgsSymbolTable()
        );

    [Test]
    public void VariableSymbolAnalyzeTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var symbolTable = CreateAggregateSymbolTable();
        var ast = ParseTestUtility.Parse( TestDataDirectory, "VariableSymbolTest.txt" );
        var symbolAnalyzer = new SymbolCollector( compilerMessageManger, symbolTable );

        Assert.DoesNotThrow( () => { symbolAnalyzer.Traverse( ast ); } );
        compilerMessageManger.WriteTo( Console.Out );
        Assert.IsTrue( symbolTable.Variables.Count == 1 );
    }

    [Test]
    public void CallbackSymbolAnalyzeTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var symbolTable = CreateAggregateSymbolTable();
        var ast = ParseTestUtility.Parse( TestDataDirectory, "CallbackSymbolTest.txt" );
        var symbolAnalyzer = new SymbolCollector( compilerMessageManger, symbolTable );

        Assert.DoesNotThrow( () => { symbolAnalyzer.Traverse( ast ); } );
        compilerMessageManger.WriteTo( Console.Out );
        Assert.IsTrue( symbolTable.UserCallbacks.Count == 1 );
    }

    [Test]
    public void UserFunctionSymbolAnalyzeTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var symbolTable = CreateAggregateSymbolTable();
        var ast = ParseTestUtility.Parse( TestDataDirectory, "UserFunctionSymbolTest.txt" );
        var symbolAnalyzer = new SymbolCollector( compilerMessageManger, symbolTable );

        Assert.DoesNotThrow( () => { symbolAnalyzer.Traverse( ast ); } );
        compilerMessageManger.WriteTo( Console.Out );
        Assert.IsTrue( compilerMessageManger.Count() == 0 );
        Assert.IsTrue( symbolTable.UserFunctions.Count == 1 );
    }
}
