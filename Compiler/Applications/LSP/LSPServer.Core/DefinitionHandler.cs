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
    private AstNodeFinder AstNodeFinder { get; } = new();

    public DefinitionHandler( CompilerCache compilerCache )
    {
        CompilerCache = compilerCache;
    }

    public async Task<LocationOrLocationLinks?> Handle( DefinitionParams request, CancellationToken cancellationToken )
    {
        var cache = CompilerCache.GetCache( request.TextDocument.Uri );
        var word = DocumentUtility.ExtractWord( cache.AllLinesText, request.Position );

        if( !AstNodeFinder.TryFind( word, cache.Ast, out var node ) )
        {
            return null;
        }

        await Task.CompletedTask;
        var definition = new Location
        {
            Uri = request.TextDocument.Uri,
            Range = new Range()
            {
                Start = node.VariableNamePosition.BeginAs(),
                End   = node.VariableNamePosition.EndAs()
            }
        };

        return new LocationOrLocationLinks( definition );
    }

    public DefinitionRegistrationOptions GetRegistrationOptions( DefinitionCapability capability, ClientCapabilities clientCapabilities )
    {
        return new DefinitionRegistrationOptions()
        {
            DocumentSelector = ConstantValues.TextDocumentSelector
        };
    }
}
