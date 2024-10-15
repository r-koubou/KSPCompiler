using System;

using KSPCompiler.Domain.Ast.Analyzers.Semantics;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;

using NUnit.Framework;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

[TestFixture]
public class AstVariableSymbolEvaluationTest
{
    private AstExpressionNode VariableSymbolTestBody( VariableSymbol variable, AbortTraverseToken abortTraverseToken )
    {
        var visitor = new MockAstSymbolVisitor();
        var compilerMessageManger = ICompilerMessageManger.Default;
        var symbolTable = MockUtility.CreateAggregateSymbolTable();

        symbolTable.Variables.Add( variable );

        var symbolEvaluator = new SymbolEvaluator( compilerMessageManger, symbolTable );
        visitor.Inject( symbolEvaluator );

        var node = MockUtility.CreateSymbolNode( variable.Name );
        var result = visitor.Visit( node, abortTraverseToken );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.IsFalse( abortTraverseToken.Aborted );

        return (AstExpressionNode)result;
    }

    [TestCase( false )]
    [TestCase( true )]
    public void IntVariableSymbolTest( bool isConstant )
    {
        var abortTraverseToken = new AbortTraverseToken();
        var variable = MockUtility.CreateIntVariable( "$x" );

        if( isConstant )
        {
            variable.DataTypeModifier |= DataTypeModifierFlag.Const;
            variable.Value            =  1;
        }

        var result = VariableSymbolTestBody( variable, abortTraverseToken );

        Assert.IsFalse( abortTraverseToken.Aborted );

        Assert.That(
            result,
            isConstant
                ? Is.InstanceOf<AstIntLiteralNode>()
                : Is.InstanceOf<AstExpressionNode>()
        );

        Assert.IsTrue( result.TypeFlag.IsInt() );
    }
}
