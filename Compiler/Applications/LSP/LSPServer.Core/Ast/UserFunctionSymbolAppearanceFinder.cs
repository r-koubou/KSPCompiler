using System.Collections.Generic;

using KSPCompiler.Commons.Text;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.LSPServer.Core.Ast;

public sealed class UserFunctionSymbolAppearanceFinder(
    string symbolName,
    AppearanceFinderMode mode = AppearanceFinderMode.All )
    : DefaultAstVisitor, IAstAppearanceFinder
{
    private string SymbolName { get; } = symbolName;
    private AppearanceFinderMode Mode { get; } = mode;
    private List<Position> Result { get; } = [];

    public IReadOnlyCollection<Position> Find( AstCompilationUnitNode ast )
    {
        Result.Clear();

        ast.AcceptChildren( this );

        return new List<Position>( Result );
    }

    public override IAstNode Visit( AstUserFunctionDeclarationNode node )
    {
        if( Mode.HasFlag( AppearanceFinderMode.Declaration ) && node.Name == SymbolName )
        {
            Result.Add( node.NamePosition );
        }

        return base.Visit( node );
    }

    public override IAstNode Visit( AstCallUserFunctionStatementNode node )
    {
        if( Mode.HasFlag( AppearanceFinderMode.Reference ) && node.Symbol.Name == SymbolName )
        {
            Result.Add( node.Symbol.Position );
        }

        return base.Visit( node );
    }
}
