using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.Applications.LSPServer.Core;

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
