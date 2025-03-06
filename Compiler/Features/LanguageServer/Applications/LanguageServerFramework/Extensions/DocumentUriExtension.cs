using EmmyLua.LanguageServer.Framework.Protocol.Model;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions;

namespace KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Extensions;

public static class DocumentUriExtension
{
    public static ScriptLocation AsScriptLocation( this DocumentUri documentUri )
    {
        return new ScriptLocation( documentUri.FileSystemPath );
    }
}
