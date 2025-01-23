using System.Collections.Generic;
using System.Collections.Immutable;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Symbols;

using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.LSPServer.Core.SemanticTokensRanges;

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

    public override IAstNode Visit( AstSymbolExpressionNode node )
    {
        if( SymbolTable.BuiltInVariables.TrySearchByName( node.Name, out var builtInVariable ) )
        {
            Result.Add( node.Position.BeginLine.Value );
            Result.Add( node.Position.BeginColumn.Value );
            Result.Add( node.Position.EndColumn.Value - node.Position.BeginColumn.Value );
            Result.Add( LegendTokenTypes.IndexOf( SemanticTokenType.Variable ) );
            Result.Add( LegendTokenModifiers.IndexOf( SemanticTokenModifier.Readonly ) );
        }

        return base.Visit( node );
    }
}
