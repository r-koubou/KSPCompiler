using System.Text;

using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Applications.LSPServer.CoreNew.Hover;

public class UITypeHoverTextBuilder : IHoverTextBuilder<UITypeSymbol>
{
    public string BuildHoverText( UITypeSymbol symbol )
    {
        var builder = new StringBuilder( 64 );

        builder.AppendLine( $"**{symbol.Name}**" );
        builder.AppendLine();
        builder.AppendLine( symbol.Description.Value );

        return builder.ToString();
    }
}
