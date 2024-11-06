using KSPCompiler.Domain.Ast.Extensions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Resources;
using KSPCompiler.UseCases.Analysis.Evaluations.UserFunctions;

namespace KSPCompiler.Interactor.Analysis.Semantics;

public class CallUserFunctionEvaluator : ICallUserFunctionEvaluator
{
    private ICompilerMessageManger CompilerMessageManger { get; }
    private IUserFunctionSymbolSymbolTable UserFunctions { get; }

    public CallUserFunctionEvaluator( ICompilerMessageManger compilerMessageManger, IUserFunctionSymbolSymbolTable symbolTable )
    {
        CompilerMessageManger = compilerMessageManger;
        UserFunctions         = symbolTable;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstCallUserFunctionStatementNode statement )
    {
        if( !UserFunctions.TrySearchByName( statement.Name, out _ ) )
        {
            CompilerMessageManger.Error(
                statement,
                CompilerMessageResources.semantic_error_userfunction_unknown,
                statement.Name
            );
        }

        return statement.Clone<AstCallUserFunctionStatementNode>();
    }
}
