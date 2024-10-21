using System.Collections.Generic;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
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

    public static AstVariableDeclarationNode CreateVariableDeclarationNode( string name )
        => new()
        {
            Name = name,
        };

    public static AstVariableDeclarationNode CreateVariableDeclarationNode( string name, AstVariableInitializerNode initializer )
        => new()
        {
            Name = name,
            Initializer = initializer
        };

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

    public static  AstSymbolExpressionNode CreateSymbolNode( string variableName, DataTypeFlag type )
    {
        return new AstSymbolExpressionNode
        {
            Name     = variableName,
            TypeFlag = type
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

    public static CallbackSymbol CreateCallback( string name, bool allowMultipleDeclaration )
        => new( allowMultipleDeclaration )
        {
            Name = name
        };

    public static CallbackSymbol CreateCallback( string name, bool allowMultipleDeclaration, params CallbackArgumentSymbol[] args )
        => new( allowMultipleDeclaration, args )
        {
            Name = name
        };

    public static AstCallbackDeclarationNode CreateCallbackDeclarationNode( string name )
    {
        return new AstCallbackDeclarationNode
        {
            Name = name,
        };
    }

    public static AstCallbackDeclarationNode CreateCallbackDeclarationNode( string name, params AstArgumentNode[] args )
    {
        var result = new AstCallbackDeclarationNode
        {
            Name = name
        };

        result.ArgumentList.Arguments.AddRange( args );

        return result;
    }

    public static UserFunctionSymbol CreateUserFunction( string name )
        => new()
        {
            Name = name,
        };

    public static AstUserFunctionDeclarationNode CreateUserFunctionDeclarationNode( string name )
    {
        return new AstUserFunctionDeclarationNode
        {
            Name = name,
        };
    }
}
