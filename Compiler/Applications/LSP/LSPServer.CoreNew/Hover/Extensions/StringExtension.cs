namespace KSPCompiler.Applications.LSPServer.CoreNew.Hover.Extensions;

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
