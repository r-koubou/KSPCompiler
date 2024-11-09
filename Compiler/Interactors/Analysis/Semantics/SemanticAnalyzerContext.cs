using KSPCompiler.Domain.CompilerMessages;
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

namespace KSPCompiler.Interactors.Analysis.Semantics;

public sealed class SemanticAnalyzerContext : IAnalyzerContext
{
    public ICompilerMessageManger CompilerMessageManger { get; }
    public AggregateSymbolTable SymbolTable { get; }

    public IDeclarationEvaluationContext DeclarationContext { get; }
    public IExpressionEvaluatorContext ExpressionContext { get; }
    public IStatementEvaluationContext StatementContext { get; }

    public SemanticAnalyzerContext(
        ICompilerMessageManger compilerMessageManger,
        AggregateSymbolTable aggregateSymbolTable )
    {
        CompilerMessageManger = compilerMessageManger;
        SymbolTable           = aggregateSymbolTable;
        DeclarationContext    = new DeclarationEvaluationContext( CompilerMessageManger, SymbolTable );
        ExpressionContext     = new ExpressionEvaluationContext( CompilerMessageManger, SymbolTable );
        StatementContext      = new StatementEvaluationContext( CompilerMessageManger, SymbolTable );
    }

    #region Declaration

    private class DeclarationEvaluationContext : IDeclarationEvaluationContext
    {
        public ICallbackDeclarationEvaluator Callback { get; }
        public IUserFunctionDeclarationEvaluator UserFunction { get; }
        public IVariableDeclarationEvaluator Variable { get; }

        public DeclarationEvaluationContext(
            ICompilerMessageManger compilerMessageManger,
            AggregateSymbolTable aggregateSymbolTable )
        {
            Callback     = new CallbackDeclarationEvaluator( compilerMessageManger, aggregateSymbolTable.ReservedCallbacks, aggregateSymbolTable.UserCallbacks );
            UserFunction = new UserFunctionDeclarationEvaluator( compilerMessageManger, aggregateSymbolTable.UserFunctions );
            Variable     = new VariableDeclarationEvaluator( compilerMessageManger, aggregateSymbolTable.Variables, aggregateSymbolTable.UITypes );
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
        public ICallCommandExpressionEvaluator CallCommand { get; }

        #endregion ~Expression Evaluators

        #region Convolution Evaluators

        public IIntegerConvolutionEvaluator IntegerConvolutionEvaluator { get; }
        public IRealConvolutionEvaluator RealConvolutionEvaluator { get; }
        public IStringConvolutionEvaluator StringConvolutionEvaluator { get; }
        public IBooleanConvolutionEvaluator BooleanConvolutionEvaluator { get; }

        #endregion ~Convolution Evaluators

        public ExpressionEvaluationContext(
            ICompilerMessageManger compilerMessageManger,
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

            AssignOperator             = new AssignOperatorEvaluator( compilerMessageManger );
            ConditionalBinaryOperator  = new ConditionalBinaryOperatorEvaluator( compilerMessageManger );
            ConditionalLogicalOperator = new ConditionalLogicalOperatorEvaluator( compilerMessageManger, BooleanConvolutionEvaluator );
            ConditionalUnaryOperator   = new ConditionalUnaryOperatorEvaluator( compilerMessageManger, BooleanConvolutionEvaluator );
            NumericBinaryOperator      = new NumericBinaryOperatorEvaluator( compilerMessageManger, IntegerConvolutionEvaluator, RealConvolutionEvaluator );
            NumericUnaryOperator       = new NumericUnaryOperatorEvaluator( compilerMessageManger, IntegerConvolutionEvaluator, RealConvolutionEvaluator );
            StringConcatenateOperator  = new StringConcatenateOperatorEvaluator( compilerMessageManger, StringConvolutionEvaluator );
            Symbol                     = new SymbolEvaluator( compilerMessageManger, aggregateSymbolTable );
            ArrayElement               = new ArrayElementEvaluator( compilerMessageManger, aggregateSymbolTable.Variables );
            CallCommand                = new CallCommandExpressionEvaluator( compilerMessageManger, aggregateSymbolTable.Commands, aggregateSymbolTable.UITypes );
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
            ICompilerMessageManger compilerMessageManger,
            AggregateSymbolTable aggregateSymbolTable )
        {
            Preprocess       = new PreprocessEvaluator();
            CallUserFunction = new CallUserFunctionEvaluator( compilerMessageManger, aggregateSymbolTable.UserFunctions );
            If               = new IfStatementEvaluator( compilerMessageManger );
            Select           = new SelectStatementEvaluator( compilerMessageManger );
            While            = new WhileStatementEvaluator( compilerMessageManger );
            Continue         = new ContinueStatementEvaluator( compilerMessageManger );
        }
    }
}
