using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Interactors.Analysis.Extensions;
using KSPCompiler.LSPServer.Core.Compilations;
using KSPCompiler.LSPServer.Core.Extensions;

using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.LSPServer.Core.Highlights;

public class DocumentHighlightHandler : IDocumentHighlightHandler
{
    private CompilerCacheService CompilerCacheService { get; }

    public DocumentHighlightHandler( CompilerCacheService compilerCacheService )
    {
        CompilerCacheService = compilerCacheService;
    }

    public async Task<DocumentHighlightContainer?> Handle( DocumentHighlightParams request, CancellationToken cancellationToken )
    {
        var cache = CompilerCacheService.GetCache( request.TextDocument.Uri );
        var word = DocumentUtility.ExtractWord( cache.AllLinesText, request.Position );
        var highlights = new List<DocumentHighlight>();

        // ユーザー定義変数
        if( cache.SymbolTable.UserVariables.TrySearchByName( word, out var variable ) )
        {
            var finder = new DocumentHighlightFinder( cache.Ast, cache.SymbolTable, variable );

            highlights.Add(
                new DocumentHighlight
                {
                    Range = variable.DefinedPosition.AsRange(),
                    Kind  = DocumentHighlightKind.Text
                }
            );

            highlights.AddRange( finder.Find() );
        }
        else if( cache.SymbolTable.UserFunctions.TrySearchByName( word, out var function ) )
        {
            var finder = new DocumentHighlightFinder( cache.Ast, cache.SymbolTable, function );

            highlights.Add(
                new DocumentHighlight
                {
                    Range = function.DefinedPosition.AsRange(),
                    Kind  = DocumentHighlightKind.Text
                }
            );

            highlights.AddRange( finder.Find() );
        }

        await Task.CompletedTask;

        return new DocumentHighlightContainer( highlights );
    }

    public DocumentHighlightRegistrationOptions GetRegistrationOptions( DocumentHighlightCapability capability, ClientCapabilities clientCapabilities )
        => new()
        {
            DocumentSelector = ConstantValues.TextDocumentSelector
        };


    private class DocumentHighlightFinder(AstCompilationUnitNode ast, AggregateSymbolTable symbolTable, SymbolBase symbolAtCursor) : DefaultAstVisitor
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
            if( !SymbolTable.TrySearchVariableByName( node.Name, out var v ) )
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

            return base.Visit( node );        }
    }
}
