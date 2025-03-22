using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Context;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Commands;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Convolutions.Booleans;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Convolutions.Integers;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Convolutions.Reals;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Convolutions.Strings;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Declarations;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Operators;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Preprocessing;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Statements;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Symbols;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.UserFunctions;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations.Convolutions.Booleans;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations.Convolutions.Conditions;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations.Convolutions.Integers;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations.Convolutions.Reals;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations.Convolutions.Strings;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.EventEmitting;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics;

public sealed class SemanticAnalyzerContext : IAnalyzerContext
{
    public IEventEmitter EventEmitter { get; }
    public AggregateSymbolTable SymbolTable { get; }

    public IDeclarationEvaluationContext DeclarationContext { get; }
    public IExpressionEvaluatorContext ExpressionContext { get; }
    public IStatementEvaluationContext StatementContext { get; }

    public SemanticAnalyzerContext(
        IEventEmitter eventEmitter,
        AggregateSymbolTable aggregateSymbolTable )
    {
        EventEmitter       = eventEmitter;
        SymbolTable        = aggregateSymbolTable;
        DeclarationContext = new DeclarationEvaluationContext( EventEmitter, SymbolTable );
        ExpressionContext  = new ExpressionEvaluationContext( EventEmitter, SymbolTable );
        StatementContext   = new StatementEvaluationContext( EventEmitter, SymbolTable );
    }

    #region Declaration

    private class DeclarationEvaluationContext : IDeclarationEvaluationContext
    {
        public ICallbackDeclarationEvaluator Callback { get; }
        public IUserFunctionDeclarationEvaluator UserFunction { get; }
        public IVariableDeclarationEvaluator Variable { get; }

        public DeclarationEvaluationContext(
            IEventEmitter eventEmitter,
            AggregateSymbolTable aggregateSymbolTable )
        {
            Callback     = new CallbackDeclarationEvaluator( eventEmitter, aggregateSymbolTable );
            UserFunction = new UserFunctionDeclarationEvaluator( eventEmitter, aggregateSymbolTable );
            Variable     = new VariableDeclarationEvaluator( eventEmitter, aggregateSymbolTable );
        }
    }

    #endregion

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
            IEventEmitter eventEmitter,
            AggregateSymbolTable aggregateSymbolTable )
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

            AssignOperator             = new AssignOperatorEvaluator( eventEmitter, aggregateSymbolTable );
            ConditionalBinaryOperator  = new ConditionalBinaryOperatorEvaluator( eventEmitter );
            ConditionalLogicalOperator = new ConditionalLogicalOperatorEvaluator( eventEmitter, BooleanConvolutionEvaluator );
            ConditionalUnaryOperator   = new ConditionalUnaryOperatorEvaluator( eventEmitter, BooleanConvolutionEvaluator );
            NumericBinaryOperator      = new NumericBinaryOperatorEvaluator( eventEmitter, aggregateSymbolTable, IntegerConvolutionEvaluator, RealConvolutionEvaluator );
            NumericUnaryOperator       = new NumericUnaryOperatorEvaluator( eventEmitter, aggregateSymbolTable, IntegerConvolutionEvaluator, RealConvolutionEvaluator );
            StringConcatenateOperator  = new StringConcatenateOperatorEvaluator( eventEmitter, StringConvolutionEvaluator );
            Symbol                     = new SymbolEvaluator( eventEmitter, aggregateSymbolTable );
            ArrayElement               = new ArrayElementEvaluator( eventEmitter, aggregateSymbolTable );
            CallCommand                = new CallCommandEvaluator( eventEmitter, aggregateSymbolTable );
        }
    }

    #endregion

    private class StatementEvaluationContext : IStatementEvaluationContext
    {
        public IPreprocessEvaluator Preprocess { get; }
        public ICallUserFunctionEvaluator CallUserFunction { get; }
        public IIfStatementEvaluator If { get; }
        public ISelectStatementEvaluator Select { get; }
        public IWhileStatementEvaluator While { get; }
        public IContinueStatementEvaluator Continue { get; }

        public StatementEvaluationContext(
            IEventEmitter eventEmitter,
            AggregateSymbolTable aggregateSymbolTable )
        {
            Preprocess       = new PreprocessEvaluator();
            CallUserFunction = new CallUserFunctionEvaluator( eventEmitter, aggregateSymbolTable );
            If               = new IfStatementEvaluator( eventEmitter );
            Select           = new SelectStatementEvaluator( eventEmitter );
            While            = new WhileStatementEvaluator( eventEmitter );
            Continue         = new ContinueStatementEvaluator( eventEmitter );
        }
    }
}
