using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Booleans;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Conditions;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Integers;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Reals;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public static class MockUtility
{
    #region Varieble

    public static VariableSymbol CreateBuiltInIntVariable( string name )
    {
        var variable = new VariableSymbol
        {
            Name     = name,
            DataType = DataTypeFlag.TypeInt,
            Modifier = ModifierFlag.Const,
            Reserved = true,
            Value    = 0
        };

        return variable;
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
            Name        = name,
            Initializer = initializer
        };

    public static UITypeSymbol CreateNoInitializerUI( string uiName, DataTypeFlag type )
    {
        return new UITypeSymbol( true )
        {
            Name     = uiName,
            Modifier = ModifierFlag.UI,
            Reserved = true,
            DataType = type
        };
    }

    public static UITypeSymbol CreateUILabel()
    {
        return new UITypeSymbol( true, new UIInitializerArgumentSymbol[]
        {
            new ()
            {
                Name     = "grid_width",
                DataType = DataTypeFlag.TypeInt
            },
            new ()
            {
                Name     = "grid_height",
                DataType = DataTypeFlag.TypeInt
            }
        })
        {
            Name     = "ui_label",
            Modifier = ModifierFlag.UI,
            Reserved = true,
            DataType = DataTypeFlag.TypeInt
        };
    }

    public static UITypeSymbol CreateUITable()
    {
        return new UITypeSymbol( true, new UIInitializerArgumentSymbol[]
        {
            new ()
            {
                Name     = "width",
                DataType = DataTypeFlag.TypeInt
            },
            new ()
            {
                Name     = "height",
                DataType = DataTypeFlag.TypeInt
            },
            new ()
            {
                Name     = "range",
                DataType = DataTypeFlag.TypeInt
            }
        })
        {
            Name     = "ui_table",
            Modifier = ModifierFlag.UI,
            Reserved = true,
            DataType = DataTypeFlag.TypeIntArray
        };
    }

    #endregion

    #region Expression

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

    public static AstSymbolExpressionNode CreateAstSymbolExpression( VariableSymbol variable )
    {
        return new AstSymbolExpressionNode( variable.Name, NullAstExpressionNode.Instance )
        {
            TypeFlag = variable.DataType,
            Constant = variable.Modifier.IsConstant()
        };
    }

    public static  AstSymbolExpressionNode CreateSymbolNode( string variableName )
    {
        return new AstSymbolExpressionNode
        {
            Name = variableName
        };
    }

    #endregion

    #region Command

    public static CommandSymbol CreateCommand( string name, DataTypeFlag returnType, params CommandArgumentSymbol[] args )
        => new( args )
        {
            Name     = name,
            DataType = returnType,
            Reserved = true
        };

    public static CommandSymbol CreatePlayNoteCommand()
    {
        var result = CreateCommand(
            "play_note",
            DataTypeFlag.TypeVoid,
            new CommandArgumentSymbol
            {
                Name     = "note_number",
                DataType = DataTypeFlag.TypeInt
            },
            new CommandArgumentSymbol
            {
                Name     = "velocity",
                DataType = DataTypeFlag.TypeInt
            },
            new CommandArgumentSymbol
            {
                Name     = "sample_offset",
                DataType = DataTypeFlag.TypeInt
            },
            new CommandArgumentSymbol
            {
                Name     = "duration",
                DataType = DataTypeFlag.TypeInt
            }
        );

        return result;
    }

    public static AstCallCommandExpressionNode CreateCommandExpressionNode( string name, params AstExpressionNode[] arguments )
    {
        var commandExpression = new AstCallCommandExpressionNode();

        var commandSymbol = new AstSymbolExpressionNode
        {
            Parent = commandExpression,
            Name = name
        };

        commandExpression.Left = commandSymbol;

        var args = new AstExpressionListNode( commandExpression )
        {
            Parent = commandExpression
        };

        foreach( var arg in arguments )
        {
            args.Expressions.Add( arg );
        }

        commandExpression.Right = args;

        return commandExpression;
    }

    #endregion

    #region Symbol Table

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

    #endregion ~Symbol Table

    #region Callback

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

    #endregion

    #region User Function

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

    #endregion

    #region Convolutions

    public static IBooleanConvolutionEvaluator CreateBooleanConvolutionEvaluator( IAstVisitor visitor )
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var symbols = MockUtility.CreateAggregateSymbolTable();

        var integerConvolutionEvaluator = new IntegerConvolutionEvaluator();
        var integerConditionalBinaryOperatorConvolutionCalculator = new IntegerConditionalBinaryOperatorConvolutionCalculator( integerConvolutionEvaluator );

        var realConvolutionEvaluator = new RealConvolutionEvaluator( visitor, symbols.Variables, compilerMessageManger );
        var realConditionalBinaryOperatorConvolutionCalculator = new RealConditionalBinaryOperatorConvolutionCalculator( realConvolutionEvaluator );

        return  new BooleanConvolutionEvaluator(
            visitor,
            integerConditionalBinaryOperatorConvolutionCalculator,
            realConditionalBinaryOperatorConvolutionCalculator
        );
    }

    #endregion ~Convolutions
}
