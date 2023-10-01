using KSPCompiler.Domain.Ast.Node.Blocks;
using KSPCompiler.Domain.Ast.Node.Statements;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Domain.Analyses.Symbol;

public interface ISymbolCollector
{
    SymbolTable<VariableSymbol> VariableTable { get; }
    SymbolTable<UserFunctionSymbol> UserFunctionTable { get; }

    ISymbolFactory<AstVariableDeclaration, VariableSymbol> VariableSymbolFactory { get; }
    ISymbolFactory<AstUserFunctionDeclaration, UserFunctionSymbol> UserFunctionSymbolFactory { get; }

    void Collect( AstCompilationUnit root );
}
