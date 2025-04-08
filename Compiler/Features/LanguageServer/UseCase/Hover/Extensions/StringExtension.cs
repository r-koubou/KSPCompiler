using KSPCompiler.Features.LanguageServer.UseCase.Abstractions;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Hover;

namespace KSPCompiler.Features.LanguageServer.UseCase.Hover.Extensions;

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
