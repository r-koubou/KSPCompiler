using System.Collections.Generic;

using KSPCompiler.Applications.LSPServer.Omnisharp.Extensions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Interactors.Analysis.Extensions;

using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.Applications.LSPServer.Omnisharp.DocumentHighlights;

public class DocumentHighlightFinder(AstCompilationUnitNode ast, AggregateSymbolTable symbolTable, SymbolBase symbolAtCursor) : DefaultAstVisitor
{
    private AstCompilationUnitNode Ast { get; } = ast;
    private AggregateSymbolTable SymbolTable { get; } = symbolTable;
    private SymbolBase SymbolAtCursor { get; } = symbolAtCursor;
    private List<DocumentHighlight> Highlights { get; } = new();

    public List<DocumentHighlight> Find()
    {
        Highlights.Clear();
        Ast.AcceptChildren( this );

        return [..Highlights];
    }

    public override IAstNode Visit( AstSymbolExpressionNode node )
    {
        if( !SymbolTable.TrySearchVariableByName( node.Name, out var v ) )
        {
            return node.Clone<AstSymbolExpressionNode>();
        }

        if( v != SymbolAtCursor )
        {
            return base.Visit( node );
        }

        var parent = node.Parent;
        var kind = DocumentHighlightKind.Read;

        if( parent is AstAssignmentExpressionNode assign && assign.Left == node )
        {
            kind = DocumentHighlightKind.Write;
        }

        Highlights.Add(
            new DocumentHighlight
            {
                Range = node.Position.AsRange(),
                Kind  = kind,
            }
        );

        return base.Visit( node );
    }

    public override IAstNode Visit( AstCallUserFunctionStatementNode node )
    {
        if( !SymbolTable.TrySearchVariableByName( node.Symbol.Name, out var v ) )
        {
            return node.Clone<AstCallUserFunctionStatementNode>();
        }

        if( v != SymbolAtCursor )
        {
            return base.Visit( node );
        }

        Highlights.Add(
            new DocumentHighlight
            {
                Range = node.Position.AsRange(),
                Kind  = DocumentHighlightKind.Read,
            }
        );

        return base.Visit( node );
    }
}
