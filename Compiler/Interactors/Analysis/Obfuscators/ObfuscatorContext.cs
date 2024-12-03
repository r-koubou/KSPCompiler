using System.Text;

using KSPCompiler.Domain.Events;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Interactors.Analysis.Commons.Evaluations.Convolutions.Booleans;
using KSPCompiler.Interactors.Analysis.Commons.Evaluations.Convolutions.Conditions;
using KSPCompiler.Interactors.Analysis.Commons.Evaluations.Convolutions.Integers;
using KSPCompiler.Interactors.Analysis.Commons.Evaluations.Convolutions.Reals;
using KSPCompiler.Interactors.Analysis.Commons.Evaluations.Convolutions.Strings;
using KSPCompiler.UseCases.Analysis.Context;
using KSPCompiler.UseCases.Analysis.Evaluations.Commands;
using KSPCompiler.UseCases.Analysis.Evaluations.Conditionals;
using KSPCompiler.UseCases.Analysis.Evaluations.Convolutions.Booleans;
using KSPCompiler.UseCases.Analysis.Evaluations.Convolutions.Integers;
using KSPCompiler.UseCases.Analysis.Evaluations.Convolutions.Reals;
using KSPCompiler.UseCases.Analysis.Evaluations.Convolutions.Strings;
using KSPCompiler.UseCases.Analysis.Evaluations.Declarations;
using KSPCompiler.UseCases.Analysis.Evaluations.Operators;
using KSPCompiler.UseCases.Analysis.Evaluations.Preprocessing;
using KSPCompiler.UseCases.Analysis.Evaluations.Symbols;
using KSPCompiler.UseCases.Analysis.Evaluations.UserFunctions;

namespace KSPCompiler.Interactors.Analysis.Obfuscators;

public sealed class ObfuscatorContext : IAnalyzerContext
{
    public IEventEmitter EventEmitter { get; }
    public AggregateSymbolTable SymbolTable { get; }
    public IDeclarationEvaluationContext DeclarationContext { get; }
    public IExpressionEvaluatorContext ExpressionContext { get; }
    public IStatementEvaluationContext StatementContext { get; }

    public ObfuscatorContext( StringBuilder output, IEventEmitter eventEmitter, AggregateSymbolTable symbolTable )
    {
        var obfuscatedVariables = new ObfuscatedVariableSymbolTable( symbolTable.Variables, "v" );
        var obfuscatedUserFunctions = new ObfuscatedUserFunctionSymbolTable( symbolTable.UserFunctions, "f" );
        var aggregateObfuscatedSymbols = new AggregateObfuscatedSymbolTable( obfuscatedVariables, obfuscatedUserFunctions );

        EventEmitter       = eventEmitter;
        SymbolTable        = symbolTable;
        DeclarationContext = new DeclarationEvaluationContext( output, symbolTable, aggregateObfuscatedSymbols );
        ExpressionContext  = new ExpressionEvaluationContext( output, symbolTable, aggregateObfuscatedSymbols );
        StatementContext   = new StatementEvaluationContext( output, aggregateObfuscatedSymbols );
    }

    #region Declaration

    private class DeclarationEvaluationContext : IDeclarationEvaluationContext
    {
        public ICallbackDeclarationEvaluator Callback { get; }
        public IUserFunctionDeclarationEvaluator UserFunction { get; }
        public IVariableDeclarationEvaluator Variable { get; }

        public DeclarationEvaluationContext(
            StringBuilder output,
            AggregateSymbolTable aggregateSymbolTable,
            AggregateObfuscatedSymbolTable aggregateObfuscatedSymbols )
        {
            Callback     = new CallbackDeclarationEvaluator( output );
            UserFunction = new UserFunctionDeclarationEvaluator( output, aggregateObfuscatedSymbols.UserFunctions );
            Variable     = new VariableDeclarationEvaluator(
                output,
                aggregateSymbolTable.Variables,
                aggregateSymbolTable.UITypes,
                aggregateObfuscatedSymbols.Variables
            );
        }
    }

    #endregion ~Declaration

    #region Expression

    private class ExpressionEvaluationContext : IExpressionEvaluatorContext
    {
        #region Expression Evaluators

        public IAssignOperatorEvaluator AssignOperator { get; }
        public IBinaryOperatorEvaluator NumericBinaryOperator { get; }
        public IUnaryOperatorEvaluator NumericUnaryOperator { get; }
        public IStringConcatenateOperatorEvaluator StringConcatenateOperator { get; }
        public IConditionalBinaryOperatorEvaluator ConditionalBinaryOperator { get; }
        public IConditionalLogicalOperatorEvaluator ConditionalLogicalOperator { get; }
        public IUnaryOperatorEvaluator ConditionalUnaryOperator { get; }
        public ISymbolEvaluator Symbol { get; }
        public IArrayElementEvaluator ArrayElement { get; }
        public ICallCommandEvaluator CallCommand { get; }

        #endregion ~Expression Evaluators

        #region Convolution Evaluators

        public IIntegerConvolutionEvaluator IntegerConvolutionEvaluator { get; }
        public IRealConvolutionEvaluator RealConvolutionEvaluator { get; }
        public IStringConvolutionEvaluator StringConvolutionEvaluator { get; }
        public IBooleanConvolutionEvaluator BooleanConvolutionEvaluator { get; }

        #endregion ~Convolution Evaluators

        public ExpressionEvaluationContext(
            StringBuilder output,
            AggregateSymbolTable aggregateSymbolTable,
            AggregateObfuscatedSymbolTable obfuscatedSymbols )
        {
            #region Convolutions

            IntegerConvolutionEvaluator = new IntegerConvolutionEvaluator();
            RealConvolutionEvaluator    = new RealConvolutionEvaluator();
            StringConvolutionEvaluator  = new StringConvolutionEvaluator();
            BooleanConvolutionEvaluator = new BooleanConvolutionEvaluator(
                new IntegerConditionalBinaryOperatorConvolutionCalculator( IntegerConvolutionEvaluator ),
                new RealConditionalBinaryOperatorConvolutionCalculator( RealConvolutionEvaluator )
            );

            #endregion ~Convolutions

            AssignOperator             = new AssignOperatorEvaluator( output );
            ConditionalBinaryOperator  = new ConditionalBinaryOperatorEvaluator( output );
            ConditionalLogicalOperator = new ConditionalLogicalOperatorEvaluator( output );
            ConditionalUnaryOperator   = new ConditionalUnaryOperatorEvaluator( output );
            NumericBinaryOperator      = new NumericBinaryOperatorEvaluator( output );
            NumericUnaryOperator       = new NumericUnaryOperatorEvaluator( output );
            StringConcatenateOperator  = new StringConcatenateOperatorEvaluator( output );
            Symbol                     = new SymbolEvaluator( output, aggregateSymbolTable, obfuscatedSymbols.Variables );
            ArrayElement               = new ArrayElementEvaluator( output );
            CallCommand                = new CallCommandEvaluator( output );
        }
    }

    #endregion ~Expression

    #region Statement

    private class StatementEvaluationContext : IStatementEvaluationContext
    {
        public IPreprocessEvaluator Preprocess { get; }
        public ICallUserFunctionEvaluator CallUserFunction { get; }
        public IIfStatementEvaluator If { get; }
        public ISelectStatementEvaluator Select { get; }
        public IWhileStatementEvaluator While { get; }
        public IContinueStatementEvaluator Continue { get; }

        public StatementEvaluationContext(
            StringBuilder output,
            AggregateObfuscatedSymbolTable obfuscatedSymbols )
        {
            Preprocess       = new PreprocessEvaluator( output );
            CallUserFunction = new CallUserFunctionEvaluator( output, obfuscatedSymbols.UserFunctions );
            If               = new IfStatementEvaluator( output );
            Select           = new SelectStatementEvaluator( output );
            While            = new WhileStatementEvaluator( output );
            Continue         = new ContinueStatementEvaluator( output );
        }
    }

    #endregion ~Statement
}
