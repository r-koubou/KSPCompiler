using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Commands;
using KSPCompiler.Domain.Ast.Extensions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Resources;

namespace KSPCompiler.Domain.Ast.Analyzers.Semantics;

public class CallCommandExpressionEvaluator : ICallCommandExpressionEvaluator
{
    private ICompilerMessageManger CompilerMessageManger { get; }
    private ICommandSymbolTable Commands { get; }

    public CallCommandExpressionEvaluator( ICompilerMessageManger compilerMessageManger, ICommandSymbolTable commands )
    {
        CompilerMessageManger = compilerMessageManger;
        Commands              = commands;
    }

    public IAstNode Evaluate( IAstVisitor<IAstNode> visitor, AstCallCommandExpressionNode expr )
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

    private bool TryGetCommandSymbol( IAstVisitor<IAstNode> visitor, AstCallCommandExpressionNode expr, out CommandSymbol result )
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

    private bool ValidateCommandArguments( IAstVisitor<IAstNode> visitor, AstCallCommandExpressionNode expr, object commandSymbol )
    {
        throw new System.NotImplementedException();
    }
}
