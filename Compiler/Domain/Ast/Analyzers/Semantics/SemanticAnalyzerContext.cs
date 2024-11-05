using KSPCompiler.Domain.Ast.Analyzers.Context;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Commands;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Conditionals;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Booleans;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Conditions;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Integers;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Reals;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Strings;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Declarations;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Operators;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Preprocessing;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Symbols;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.UserFunctions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Domain.Ast.Analyzers.Semantics;

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
            CallCommand                = new CallCommandExpressionEvaluator( compilerMessageManger, aggregateSymbolTable.Commands );
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
