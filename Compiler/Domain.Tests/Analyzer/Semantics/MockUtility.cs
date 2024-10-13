using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public static class MockUtility
{
    public static VariableSymbol CreateIntVariable( string name )
    {
        var variable = new VariableSymbol
        {
            Name     = name,
            DataType = DataTypeFlag.TypeReal
        };

        return variable;
    }

    public static  T CreateBinaryOperatorNode<T>( string variableName, DataTypeFlag leftType, AstExpressionNode right ) where T : AstExpressionNode, new()
    {
        return new T
        {
            Left = new AstSymbolExpressionNode
            {
                Name     = variableName,
                TypeFlag = leftType
            },
            Right = right
        };
    }

    public static  AstUnaryMinusExpressionNode CreateUnaryMinusOperatorNode( string variableName, DataTypeFlag type )
    {
        return new AstUnaryMinusExpressionNode
        {
            Left = new AstSymbolExpressionNode
            {
                Name     = variableName,
                TypeFlag = type
            },
        };
    }

    public static  AstUnaryNotExpressionNode CreateUnaryNotOperatorNode( string variableName, DataTypeFlag type )
    {
        return new AstUnaryNotExpressionNode
        {
            Left = new AstSymbolExpressionNode
            {
                Name     = variableName,
                TypeFlag = type
            },
        };
    }
}
