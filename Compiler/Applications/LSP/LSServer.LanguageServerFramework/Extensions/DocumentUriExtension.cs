using EmmyLua.LanguageServer.Framework.Protocol.Model;

using KSPCompiler.UseCases.LanguageServer;

namespace KSPCompiler.Applications.LSServer.LanguageServerFramework.Extensions;

public static class DocumentUriExtension
{
    public static ScriptLocation AsScriptLocation( this DocumentUri uri )
    {
        return new ScriptLocation( uri.Uri.AbsolutePath );
    }
}
