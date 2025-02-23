using System;

using EmmyLua.LanguageServer.Framework.Protocol.Model;

using KSPCompiler.Applications.LSPServer.CoreNew;

namespace KSPCompiler.Applications.LSServer.LanguageServerFramework.Extensions;

public static class ScriptLocationExtension
{
    public static DocumentUri AsDocumentUri( this ScriptLocation self )
    {
        return new DocumentUri( new Uri( self.Value, uriKind: UriKind.RelativeOrAbsolute ) );
    }
}
