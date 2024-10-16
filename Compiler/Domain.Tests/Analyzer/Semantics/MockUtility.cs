using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public static class MockUtility
{
    public static VariableSymbol CreateBuiltInIntVariable( string name )
    {
        var variable = new VariableSymbol
        {
            Name             = name,
            DataType         = DataTypeFlag.TypeInt,
            DataTypeModifier = DataTypeModifierFlag.Const,
            Reserved         = true,
            Value            = 0
        };

        return variable;
    }

    public static AstSymbolExpressionNode CreateAstSymbolExpression( VariableSymbol variable )
    {
        return new AstSymbolExpressionNode( variable.Name, NullAstExpressionNode.Instance )
        {
            TypeFlag = variable.DataType,
            Constant = variable.DataTypeModifier.IsConstant()
        };
    }

    public static VariableSymbol CreateVariable( string name, DataTypeFlag type )
    {
        var variable = new VariableSymbol
        {
            Name     = name,
            DataType = type
        };

        return variable;
    }

    public static VariableSymbol CreateIntVariable( string name )
        => CreateVariable( name, DataTypeFlag.TypeInt );

    public static VariableSymbol CreateRealVariable( string name )
        => CreateVariable( name, DataTypeFlag.TypeReal );

    public static VariableSymbol CreateStringVariable( string name )
        => CreateVariable( name, DataTypeFlag.TypeString );

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

    public static  AstSymbolExpressionNode CreateSymbolNode( string variableName )
    {
        return new AstSymbolExpressionNode
        {
            Name = variableName
        };
    }

    public static AggregateSymbolTable CreateAggregateSymbolTable()
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
}
