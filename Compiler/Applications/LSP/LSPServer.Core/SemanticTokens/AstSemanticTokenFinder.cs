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

    public ImmutableArray<int> Find( AstCompilationUnitNode ast )
    {
        Result.Clear();
        ast.AcceptChildren( this );

        return ImmutableArray.Create( Result.ToArray() );
    }

    private void Add( KSPCompilerPosition position, SemanticTokenType tokenType, SemanticTokenModifier tokenModifier )
    {
        Result.Add( position.BeginLine.Value - 1 );
        Result.Add( position.BeginColumn.Value );
        Result.Add( position.EndColumn.Value - position.BeginColumn.Value );
        Result.Add( LegendTokenTypes.IndexOf( tokenType ) );
        Result.Add( LegendTokenModifiers.IndexOf( tokenModifier ) );
    }

    private void Add( KSPCompilerPosition begin, KSPCompilerPosition end, SemanticTokenType tokenType, SemanticTokenModifier tokenModifier )
    {
        Result.Add( begin.BeginLine.Value - 1 );
        Result.Add( begin.BeginColumn.Value );
        Result.Add( end.EndColumn.Value - begin.BeginColumn.Value );
        Result.Add( LegendTokenTypes.IndexOf( tokenType ) );
        Result.Add( LegendTokenModifiers.IndexOf( tokenModifier ) );
    }

    public override IAstNode Visit( AstSymbolExpressionNode node )
    {
        if( SymbolTable.BuiltInVariables.TrySearchByName( node.Name, out var builtInVariable ) )
        {
            Add( node.Position, SemanticTokenType.Variable, SemanticTokenModifier.DefaultLibrary );
        }

        return base.Visit( node );
    }

    public override IAstNode Visit( AstCallCommandExpressionNode node )
    {
        if( SymbolTable.Commands.TrySearchByName( node.Left.Name, out var commandSymbol ) )
        {
            Add( node.Position, SemanticTokenType.Method, SemanticTokenModifier.DefaultLibrary );
        }

        return base.Visit( node );
    }

    public override IAstNode Visit( AstCallbackDeclarationNode node )
    {
        if( SymbolTable.UserCallbacks.TrySearchByName( node.Name, out _ ) )
        {
            // on
            Add( node.BeginOnPosition, SemanticTokenType.Keyword, SemanticTokenModifier.DefaultLibrary );

            // callback name
            Add( node.NamePosition, SemanticTokenType.Method, SemanticTokenModifier.DefaultLibrary );

            // end on
            Add( node.EndPosition, node.EndOnPosition, SemanticTokenType.Keyword, SemanticTokenModifier.DefaultLibrary );
        }

        return base.Visit( node );
    }
}
