using System.Linq;
using System.Text;

using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.LanguageServer.UseCase.Hover;

public class UserDefinedSymbolHoverTextBuilder<TSymbol> : IHoverTextBuilder<TSymbol> where TSymbol : SymbolBase
{
    public string BuildHoverText( TSymbol symbol )
    {
        var builder = new StringBuilder( 64 );
        var commentLines = symbol.CommentLines;

        var minIndent = commentLines
                       .Where( line => line.Trim().Length > 0 )
                       .Select( line => line.Length - line.TrimStart().Length )
                       .DefaultIfEmpty( 0 )
                       .Min();

        var normalizedLines = commentLines
           .Select( line => line.Length >= minIndent ? line.Substring( minIndent ) : line );

        foreach( var line in normalizedLines )
        {
            builder.AppendLine( line );
        }

        return builder.ToString();
    }
}
