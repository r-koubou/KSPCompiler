using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.LSPServer.Core.Ast;
using KSPCompiler.LSPServer.Core.Extensions;

using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.LSPServer.Core;

public class DefinitionHandler : IDefinitionHandler
{
    private CompilerCache CompilerCache { get; }
    private AsteclarationNodeFinder AsteclarationNodeFinder { get; } = new();

    public DefinitionHandler( CompilerCache compilerCache )
    {
        CompilerCache = compilerCache;
    }

    #region IDefinitionHandler
    public async Task<LocationOrLocationLinks?> Handle( DefinitionParams request, CancellationToken cancellationToken )
    {
        var cache = CompilerCache.GetCache( request.TextDocument.Uri );
        var word = DocumentUtility.ExtractWord( cache.AllLinesText, request.Position );

        if( TryDefinitionLocateByVariableName( request, word, cache, out var result ) )
        {
            return new LocationOrLocationLinks( result );
        }

        if( TryDefinitionLocateByUserFunctionName( request, word, cache, out result ) )
        {
            return new LocationOrLocationLinks( result );
        }

        await Task.CompletedTask;
        return null;
    }

    public DefinitionRegistrationOptions GetRegistrationOptions( DefinitionCapability capability, ClientCapabilities clientCapabilities )
    {
        return new DefinitionRegistrationOptions()
        {
            DocumentSelector = ConstantValues.TextDocumentSelector
        };
    }
    #endregion ~IDefinitionHandler

    #region Common Logic
    private bool TryDefinitionLocateByVariableName( DefinitionParams request, string word, CompilerCacheItem cache, out Location result )
    {
        result = null!;

        if( !AsteclarationNodeFinder.TryFindVariableDeclarationNode( word, cache.Ast, out var node ) )
        {
            return false;
        }

        result = new Location
        {
            Uri = request.TextDocument.Uri,
            Range = new Range()
            {
                Start = node.VariableNamePosition.BeginAs(),
                End   = node.VariableNamePosition.EndAs()
            }
        };

        return true;
    }

    private bool TryDefinitionLocateByUserFunctionName( DefinitionParams request, string word, CompilerCacheItem cache, out Location result )
    {
        result = null!;

        if( !AsteclarationNodeFinder.TryFindUserFunctionDeclarationNode( word, cache.Ast, out var node ) )
        {
            return false;
        }

        result = new Location
        {
            Uri = request.TextDocument.Uri,
            Range = new Range()
            {
                Start = node.FunctionNamePosition.BeginAs(),
                End   = node.FunctionNamePosition.EndAs()
            }
        };

        return true;
    }

    #endregion

}
