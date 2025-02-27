using System;

using EmmyLua.LanguageServer.Framework.Protocol.Model;

using KSPCompiler.UseCases.LanguageServer;

namespace KSPCompiler.Applications.LSServer.LanguageServerFramework.Extensions;

public static class ScriptLocationExtension
{
    public static DocumentUri AsDocumentUri( this ScriptLocation self )
    {
        return new DocumentUri( new Uri( self.Value, uriKind: UriKind.RelativeOrAbsolute ) );
    }

    public static ScriptLocation RemoveFileSchemeString( this ScriptLocation self )
    {
        if( self.Value.StartsWith( "file://" ) )
        {
            return new ScriptLocation( self.Value[7..] );
        }

        return self;
    }

}
