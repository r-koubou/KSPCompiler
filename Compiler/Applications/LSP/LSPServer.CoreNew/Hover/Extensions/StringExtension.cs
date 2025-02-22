namespace KSPCompiler.Applications.LSPServer.CoreNew.Hover.Extensions;

public static class StringExtension
{
    public static HoverItem AsHover( this string self )
        => new()
        {
            Content = self,
            IsMarkdown = true
        };
}
