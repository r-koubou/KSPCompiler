using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Interactors.LanguageServer.Extensions;
using KSPCompiler.UseCases.LanguageServer;
using KSPCompiler.UseCases.LanguageServer.Definition;

namespace KSPCompiler.Interactors.LanguageServer.Definition;

public sealed class DefinitionHandlingInteractor : IDefinitionHandlingInteractor
{
    public async Task<DefinitionOutputPort> ExecuteAsync(
        DefinitionInputPort parameter,
        CancellationToken cancellationToken = default )
    {
        try
        {
            var compilationCacheManager = parameter.HandlingInputData.Cache;
            var scriptLocation = parameter.HandlingInputData.Location;
            var position = parameter.HandlingInputData.Position;

            if( !compilationCacheManager.ContainsCache( scriptLocation ) )
            {
                return new DefinitionOutputPort( [ ], false );
            }

            var cache = compilationCacheManager.GetCache( scriptLocation );
            var word = DocumentUtility.ExtractWord( cache.AllLinesText, position );
            var links = new List<LocationLink>();

            // ユーザー定義変数
            if( cache.SymbolTable.UserVariables.TrySearchDefinitionLocation( scriptLocation, word, out var result ) )
            {
                links.Add( result );
            }

            // ユーザー定義関数
            if( cache.SymbolTable.UserFunctions.TrySearchDefinitionLocation( scriptLocation, word, out result ) )
            {
                links.Add( result );
            }

            await Task.CompletedTask;

            return new DefinitionOutputPort( links, true );
        }
        catch( Exception e )
        {
            return new DefinitionOutputPort( [ ], false, e );
        }
    }
}
