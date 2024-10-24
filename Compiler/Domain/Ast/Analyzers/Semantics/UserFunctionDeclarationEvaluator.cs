using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Declarations;
using KSPCompiler.Domain.Ast.Extensions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.Extensions;
using KSPCompiler.Resources;

namespace KSPCompiler.Domain.Ast.Analyzers.Semantics;

public class UserFunctionDeclarationEvaluator : IUserFunctionDeclarationEvaluator
{
    private ICompilerMessageManger CompilerMessageManger { get; }
    private ISymbolTable<UserFunctionSymbol> SymbolTable { get; }

    public UserFunctionDeclarationEvaluator(
        ICompilerMessageManger compilerMessageManger,
        ISymbolTable<UserFunctionSymbol> symbolTable )
    {
        SymbolTable           = symbolTable;
        CompilerMessageManger = compilerMessageManger;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstUserFunctionDeclarationNode node )
    {
        var thisUserFunction = node.As();

        if( !SymbolTable.Add( thisUserFunction ) )
        {
            CompilerMessageManger.Error(
                node,
                CompilerMessageResources.symbol_error_declare_userfunction_already,
                node.Name
            );
        }

        return node;
    }
}
