using System.Text;

using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.LanguageServer.UseCase.Hover;

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
