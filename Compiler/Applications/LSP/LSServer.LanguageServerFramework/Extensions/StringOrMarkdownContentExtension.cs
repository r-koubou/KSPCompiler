using EmmyLua.LanguageServer.Framework.Protocol.Model.Markup;

using KSPCompiler.Applications.LSPServer.Core;

namespace KSPCompiler.Applications.LSServer.LanguageServerFramework.Extensions;

public static class StringOrMarkdownContentExtension
{
    public static MarkupContent As( this StringOrMarkdownContent self )
    {
        return new MarkupContent
        {
            Value = self.Value,
            Kind = self.IsMarkdown
                ? MarkupKind.Markdown
                : MarkupKind.PlainText
        };
    }
}
