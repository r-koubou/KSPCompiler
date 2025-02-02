using System.Collections.Generic;
using System.Collections.Immutable;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Symbols;

using OmniSharp.Extensions.LanguageServer.Protocol.Models;

using KSPCompilerPosition = KSPCompiler.Commons.Text.Position;

namespace KSPCompiler.LSPServer.Core.SemanticTokens;

public class AstSemanticTokenFinder(
    AggregateSymbolTable symbolTable,
    ImmutableArray<SemanticTokenType> legendTokenTypes,
    ImmutableArray<SemanticTokenModifier> legendTokenModifiers
) : DefaultAstVisitor
{
    private AggregateSymbolTable SymbolTable { get; } = symbolTable;
    private ImmutableArray<SemanticTokenType> LegendTokenTypes { get; } = legendTokenTypes;
    private ImmutableArray<SemanticTokenModifier> LegendTokenModifiers { get; } = legendTokenModifiers;
    private List<int> Result { get; } = [];

    private int lastLine = 0;
    private int lastColumn = 0;

    public ImmutableArray<int> Find( AstCompilationUnitNode ast )
    {
        Result.Clear();
        lastLine = 0;
        lastColumn = 0;

        ast.AcceptChildren( this );

        return ImmutableArray.Create( Result.ToArray() );
    }

    private void Add( KSPCompilerPosition position, SemanticTokenType tokenType )
    {
        var deltaLine = 0;
        var deltaColumn = 0;

        if( lastLine != position.BeginLine.Value - 1 )
        {
            deltaLine = position.BeginLine.Value - lastLine - 1;
            deltaColumn = position.BeginColumn.Value;
        }
        else
        {
            deltaColumn = position.BeginColumn.Value - lastColumn;
        }

        var length = position.EndColumn.Value - position.BeginColumn.Value;

        Result.Add( deltaLine );
        Result.Add( deltaColumn );
        Result.Add( length );
        Result.Add( LegendTokenTypes.IndexOf( tokenType ) );
        Result.Add( 0 );

        lastLine   = position.BeginLine.Value - 1;
        lastColumn = position.BeginColumn.Value;
    }


    // public override IAstNode Visit( AstCallbackDeclarationNode node )
    // {
    //     if( SymbolTable.UserCallbacks.TrySearchByName( node.Name, out _ ) )
    //     {
    //         // on
    //         Add( node.BeginOnKeywordPosition, SemanticTokenType.Keyword );
    //
    //         // callback name
    //         Add( node.NamePosition, SemanticTokenType.Keyword );
    //
    //         // end on
    //         Add( node.EndKeywordPosition, SemanticTokenType.Keyword );
    //         Add( node.EndOnKeywordPosition, SemanticTokenType.Keyword );
    //     }
    //
    //     return base.Visit( node );
    // }

    // public override IAstNode Visit( AstUserFunctionDeclarationNode node )
    // {
    //     if( SymbolTable.UserFunctions.TrySearchByName( node.Name, out _ ) )
    //     {
    //         // function
    //         Add( node.BeginOnKeywordPosition, SemanticTokenType.Keyword );
    //
    //         // function name
    //         Add( node.NamePosition, SemanticTokenType.Keyword );
    //
    //         // end function
    //         Add( node.EndKeywordPosition, SemanticTokenType.Keyword );
    //         Add( node.EndOnKeywordPosition, SemanticTokenType.Keyword );
    //     }
    //
    //     return base.Visit( node );
    // }
}
