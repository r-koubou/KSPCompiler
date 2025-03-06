using System;

using EmmyLua.LanguageServer.Framework.Protocol.Model;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions;

namespace KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Extensions;

public static class ScriptLocationExtension
{
    public static DocumentUri AsDocumentUri( this ScriptLocation self )
    {
        return new DocumentUri( new Uri( self.Value, uriKind: UriKind.RelativeOrAbsolute ) );
    }
}
