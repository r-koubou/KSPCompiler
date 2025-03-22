using System.Collections.Generic;
using System.Text;

using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.LanguageServer.UseCase.Hover.Extensions;

public static class OverloadedSymbolTableExtensionForHoverTextExtension
{
    public static bool TryBuildHoverText( this ICommandSymbolTable self, string symbolName, out string result, IOverloadedHoverTextBuilder<CommandSymbol>? builder = null )
    {
        result = string.Empty;

        if( !self.TrySearchByName( symbolName, out var symbol ) )
        {
            return false;
        }

        var stringBuilder = new StringBuilder();

        if( builder == null )
        {
            result = AppendWithNoBuilder( symbol, stringBuilder ).ToString();

            return true;
        }

        result = builder.BuildHoverText( symbol );

        return !string.IsNullOrEmpty( result );
    }

    private static StringBuilder AppendWithNoBuilder(
        IReadOnlyCollection<CommandSymbol> symbol,
        StringBuilder stringBuilder )
    {
        var i = 0;
        var length = symbol.Count;

        foreach( var overload in symbol )
        {
            if( length > 0 )
            {
                stringBuilder.Append( $"[{i+1}]" );
            }

            stringBuilder.AppendLine( overload.Description.Value );

            if( i < length - 1 )
            {
                stringBuilder.AppendLine();
            }

            i++;
        }

        return stringBuilder;
    }
}
