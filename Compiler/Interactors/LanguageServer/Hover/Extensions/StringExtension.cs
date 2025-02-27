using KSPCompiler.UseCases.LanguageServer;
using KSPCompiler.UseCases.LanguageServer.Hover;

namespace KSPCompiler.Interactors.LanguageServer.Hover.Extensions;

public static class StringExtension
{
    public static HoverItem AsHover( this string self )
        => new()
        {
            Content = new StringOrMarkdownContent
            {
                Value      = self,
                IsMarkdown = true
            }
        };
}
