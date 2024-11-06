using System.Linq;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Interactors.Analysis.Commons.Evaluations;
using KSPCompiler.Interactors.Analysis.Commons.Extensions;
using KSPCompiler.Resources;
using KSPCompiler.UseCases.Analysis.Evaluations.Commands;

namespace KSPCompiler.Interactors.Analysis.Semantics;

public class CallCommandExpressionEvaluator : ICallCommandExpressionEvaluator
{
    private ICompilerMessageManger CompilerMessageManger { get; }
    private ICommandSymbolTable Commands { get; }

    public CallCommandExpressionEvaluator( ICompilerMessageManger compilerMessageManger, ICommandSymbolTable commands )
    {
        CompilerMessageManger = compilerMessageManger;
        Commands              = commands;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstCallCommandExpressionNode expr )
    {
        if( !TryGetCommandSymbol( visitor, expr, out var commandSymbol ) )
        {
            return NullAstExpressionNode.Instance;
        }

        if( !ValidateCommandArguments( visitor, expr, commandSymbol ) )
        {
            return NullAstExpressionNode.Instance;
        }

        var result = expr.Clone<AstCallCommandExpressionNode>();
        result.TypeFlag = commandSymbol.DataType;

        return result;
    }

    private bool TryGetCommandSymbol( IAstVisitor visitor, AstCallCommandExpressionNode expr, out CommandSymbol result )
    {
        result = default!;

        if( expr.Left.Accept( visitor ) is not AstExpressionNode evaluatedSymbolExpr )
        {
            throw new AstAnalyzeException( expr, "Failed to evaluate command symbol" );
        }

        if( !Commands.TrySearchByName( evaluatedSymbolExpr.Name, out var commandSymbol ) )
        {
            CompilerMessageManger.Warning(
                expr,
                CompilerMessageResources.semantic_warning_command_unknown,
                evaluatedSymbolExpr.Name
            );

            return false;
        }

        result = commandSymbol;

        return true;
    }

    private bool ValidateCommandArguments( IAstVisitor visitor, AstCallCommandExpressionNode expr, CommandSymbol commandSymbol )
    {
        if( expr.Right is not AstExpressionListNode arguments )
        {
            throw new AstAnalyzeException( expr, "Failed to evaluate command arguments" );
        }

        var symbolArgs = commandSymbol.Arguments.ToList();
        var callArgs   = arguments.Expressions.ToList();

        if( symbolArgs.Count != callArgs.Count )
        {
            CompilerMessageManger.Error(
                expr,
                CompilerMessageResources.semantic_error_command_arg_count,
                commandSymbol.Name
            );

            return false;
        }

        for( var i = 0; i < callArgs.Count; i++ )
        {
            var symbolArg = symbolArgs[ i ];
            var callArg = callArgs[ i ];

            if( callArg.Accept( visitor ) is not AstExpressionNode evaluatedArg )
            {
                throw new AstAnalyzeException( callArg, "Failed to evaluate command argument" );
            }

            if( TypeCompatibility.IsTypeCompatible( evaluatedArg.TypeFlag, symbolArg.DataType ) )
            {
                continue;
            }

            CompilerMessageManger.Error(
                callArg,
                CompilerMessageResources.semantic_error_command_arg_incompatible,
                commandSymbol.Name,
                i + 1, // 1-based index
                symbolArg.DataType.ToMessageString(),
                callArg.TypeFlag.ToMessageString()
            );

            return false;
        }

        return true;
    }
}
