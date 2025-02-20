using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.LSPServer.Omnisharp;

public static class ConstantValues
{
    public const string LanguageId = "ksp";

    public static readonly TextDocumentSelector TextDocumentSelector = new(
        new TextDocumentFilter
        {
            Language = LanguageId
        }
    );
}
