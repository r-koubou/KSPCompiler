using KSPCompiler.Domain.Ast.Analyzers.Evaluators.KspUserFunctions;
using KSPCompiler.Domain.Ast.Extensions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Resources;

namespace KSPCompiler.Domain.Ast.Analyzers.Semantics;

public class CallKspUserFunctionStatementEvaluator : ICallKspUserFunctionEvaluator
{
    private ICompilerMessageManger CompilerMessageManger { get; }
    private IUserFunctionSymbolSymbolTable UserFunctions { get; }

    public CallKspUserFunctionStatementEvaluator( ICompilerMessageManger compilerMessageManger, IUserFunctionSymbolSymbolTable symbolTable )
    {
        CompilerMessageManger = compilerMessageManger;
        UserFunctions         = symbolTable;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstCallKspUserFunctionStatementNode statement )
    {
        if( !UserFunctions.TrySearchByName( statement.Name, out _ ) )
        {
            CompilerMessageManger.Error(
                statement,
                CompilerMessageResources.semantic_error_userfunction_ksp_unknown,
                statement.Name
            );
        }

        return statement.Clone<AstCallKspUserFunctionStatementNode>();
    }
}
