using EmmyLua.LanguageServer.Framework.Protocol.Model;

using KSPCompiler.UseCases.LanguageServer;

namespace KSPCompiler.Applications.LSServer.LanguageServerFramework.Extensions;

public static class DocumentUriExtension
{
    public static ScriptLocation AsScriptLocation( this DocumentUri documentUri )
    {
        return new ScriptLocation( documentUri.FileSystemPath );
    }
}
