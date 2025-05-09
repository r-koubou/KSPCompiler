using System;
using System.Collections.Generic;

using KSPCompiler.Features.Compilation.Domain.Messages;
using KSPCompiler.Features.Compilation.Domain.Messages.Extensions;
using KSPCompiler.Features.Compilation.Gateways.EventEmitting;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Convolutions.Booleans;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations.Convolutions.Booleans;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations.Convolutions.Conditions;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations.Convolutions.Integers;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations.Convolutions.Reals;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;
using KSPCompiler.Features.SymbolManagement.UseCase.Tests.Analysis.Semantics;
using KSPCompiler.Features.SymbolManagement.UseCase.Tests.Commons;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Statements;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData.Extensions;
using KSPCompiler.Shared.EventEmitting.Extensions;

using NUnit.Framework;

using SemanticNumericBinaryOperatorEvaluator = KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics.NumericBinaryOperatorEvaluator;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Tests.Analysis;

public static class MockUtility
{
    #region Varieble

    public static VariableSymbol CreateBuiltInIntVariable( string name )
    {
        var variable = new VariableSymbol
        {
            Name          = name,
            DataType      = DataTypeFlag.TypeInt,
            Modifier      = ModifierFlag.Const,
            BuiltIn       = true,
            ConstantValue = 0
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

    public static VariableSymbol CreateVariable( string name )
    {
        var variable = new VariableSymbol
        {
            Name     = name,
            DataType = DataTypeUtility.GuessFromSymbolName( name )
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
            BuiltIn  = true,
            DataType = type
        };
    }

    public static UITypeSymbol CreateUILabel()
    {
        return new UITypeSymbol( true, new UIInitializerArgumentSymbol[]
            {
                new()
                {
                    Name     = "grid_width",
                    DataType = DataTypeFlag.TypeInt
                },
                new()
                {
                    Name     = "grid_height",
                    DataType = DataTypeFlag.TypeInt
                }
            }
        )
        {
            Name     = "ui_label",
            Modifier = ModifierFlag.UI,
            BuiltIn  = true,
            DataType = DataTypeFlag.TypeInt
        };
    }

    public static UITypeSymbol CreateUITable()
    {
        return new UITypeSymbol( true, new UIInitializerArgumentSymbol[]
            {
                new()
                {
                    Name     = "width",
                    DataType = DataTypeFlag.TypeInt
                },
                new()
                {
                    Name     = "height",
                    DataType = DataTypeFlag.TypeInt
                },
                new()
                {
                    Name     = "range",
                    DataType = DataTypeFlag.TypeInt
                }
            }
        )
        {
            Name     = "ui_table",
            Modifier = ModifierFlag.UI,
            BuiltIn  = true,
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
                Name        = variableName,
                TypeFlag    = type
            },
        };
    }

    public static  AstUnaryNotExpressionNode CreateUnaryNotOperatorNode( string variableName, DataTypeFlag type )
    {
        return new AstUnaryNotExpressionNode
        {
            Left = new AstSymbolExpressionNode
            {
                Name        = variableName,
                TypeFlag    = type
            },
        };
    }

    public static  AstSymbolExpressionNode CreateSymbolNode( string symbolName, DataTypeFlag type )
    {
        return new AstSymbolExpressionNode
        {
            Name     = symbolName,
            TypeFlag = type
        };
    }

    public static AstSymbolExpressionNode CreateAstSymbolExpression( VariableSymbol variable )
    {
        return new AstSymbolExpressionNode( variable.Name )
        {
            TypeFlag = variable.DataType,
            Constant = variable.Modifier.IsConstant()
        };
    }

    public static  AstSymbolExpressionNode CreateSymbolNode( string symbolName )
    {
        return new AstSymbolExpressionNode
        {
            Name = symbolName
        };
    }

    #endregion

    #region Command

    public static CommandSymbol CreateCommand( string name, DataTypeFlag returnType, params CommandArgumentSymbol[] args )
        => new( args )
        {
            Name     = name,
            DataType = returnType,
            BuiltIn  = true
        };

    public static CommandSymbol CreateMessageCommand()
    {
        var result = CreateCommand(
            "message",
            DataTypeFlag.TypeVoid,
            new CommandArgumentSymbol
            {
                Name     = "message",
                DataType = DataTypeFlag.All
            }
        );

        return result;
    }

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

    public static List<CommandSymbol> CreateNoteOffCommand()
    {
        var result = new List<CommandSymbol>();

        result.Add(
            CreateCommand(
                "note_off",
                DataTypeFlag.TypeVoid,
                new CommandArgumentSymbol
                {
                    Name     = "event_id",
                    DataType = DataTypeFlag.TypeInt
                }
            )
        );

        result.Add(
            CreateCommand(
                "note_off",
                DataTypeFlag.TypeVoid,
                new CommandArgumentSymbol
                {
                    Name     = "event_id",
                    DataType = DataTypeFlag.TypeInt
                },
                new CommandArgumentSymbol
                {
                    Name     = "time_offset",
                    DataType = DataTypeFlag.TypeInt
                }
            )
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
    public static AggregateObfuscatedSymbolTable CreateAggregateObfuscatedSymbolTable( AggregateSymbolTable source, string variablePrefix = "v", string functionPrefix = "f" )
        => new(
            new ObfuscatedVariableSymbolTable( source.UserVariables, variablePrefix ),
            new ObfuscatedUserFunctionSymbolTable( source.UserFunctions, functionPrefix )
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

    #region Ksp User Function

    public static UserFunctionSymbol CreateUserFunction( string name )
        => new()
        {
            Name = name
        };

    public static AstUserFunctionDeclarationNode CreateUserFunctionDeclarationNode( string name )
    {
        return new AstUserFunctionDeclarationNode
        {
            Name = name
        };
    }

    public static AstCallUserFunctionStatementNode CreateCallUserFunctionNode( UserFunctionSymbol function )
    {
        return new AstCallUserFunctionStatementNode
        {
            Symbol = new AstSymbolExpressionNode( function.Name )
        };
    }

    public static AstCallUserFunctionStatementNode CreateCallUserFunctionNode( string name )
    {
        return new AstCallUserFunctionStatementNode
        {
            Symbol = new AstSymbolExpressionNode( name )
        };
    }

    #endregion

    #region Convolutions

    public static IBooleanConvolutionEvaluator CreateBooleanConvolutionEvaluator( IAstVisitor visitor )
    {
        var integerConvolutionEvaluator = new IntegerConvolutionEvaluator();
        var integerConditionalBinaryOperatorConvolutionCalculator = new IntegerConditionalBinaryOperatorConvolutionCalculator( integerConvolutionEvaluator );

        var realConvolutionEvaluator = new RealConvolutionEvaluator();
        var realConditionalBinaryOperatorConvolutionCalculator = new RealConditionalBinaryOperatorConvolutionCalculator( realConvolutionEvaluator );

        return new BooleanConvolutionEvaluator(
            integerConditionalBinaryOperatorConvolutionCalculator,
            realConditionalBinaryOperatorConvolutionCalculator
        );
    }

    #endregion ~Convolutions

    #region Operators

    public static void OperatorTestBody<TOperatorNode>( Func<IAstVisitor, TOperatorNode, IAstNode> visitImpl, int expectedErrorCount, DataTypeFlag expectedEvaluatedType )
        where TOperatorNode : AstExpressionNode, new()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var visitor = new MockAstBinaryOperatorVisitor();

        var binaryOperatorEvaluator = new SemanticNumericBinaryOperatorEvaluator(
            eventEmitter,
            new AggregateSymbolTable(),
            new MockIntegerConvolutionEvaluator(),
            new RealConvolutionEvaluator()
        );

        visitor.Inject( binaryOperatorEvaluator );

        var ast = new TOperatorNode();

        switch( expectedEvaluatedType )
        {
            case DataTypeFlag.TypeInt:
                ast.Left = new AstIntLiteralNode( 1 );
                ast.Right = new AstIntLiteralNode( 2 );
                break;
            case DataTypeFlag.TypeReal:
                ast.Left = new AstRealLiteralNode( 1 );
                ast.Right = new AstRealLiteralNode( 2 );
                break;
            default:
                throw new ArgumentException();
        }

        var result = visitImpl( visitor, ast ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( expectedErrorCount ) );
        Assert.That( result, Is.Not.Null );
        Assert.That( result?.TypeFlag, Is.EqualTo( expectedEvaluatedType ) );
    }

    #endregion |~Operators
}
