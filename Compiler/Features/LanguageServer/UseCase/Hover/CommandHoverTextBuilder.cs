using System.Collections.Generic;
using System.Text;

using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.LanguageServer.UseCase.Hover;

public sealed class CommandHoverTextBuilder : IOverloadedHoverTextBuilder<CommandSymbol>
{
    public string BuildHoverText( IReadOnlyCollection<CommandSymbol> symbols )
    {
        var builder = new StringBuilder( 128 );
        var count = symbols.Count;
        var i = 0;

        foreach( var x in symbols )
        {
            BuildHoverText( x, builder );
            if( i < count - 1 )
            {
                builder.AppendLine( "or" );
                builder.AppendLine();
            }
            i++;
        }

        return builder.ToString();
    }

    private void BuildHoverText( CommandSymbol symbol, StringBuilder builder )
    {
        /*
         * command_name(arg1, arg2, ...)
         * Description
         */

        builder.Append( $"**{symbol.Name}" );

        if( symbol.ArgumentCount > 0 )
        {
            var index = 0;
            var argCount = symbol.Arguments.Count;

            builder.Append( "(" );

            foreach( var arg in symbol.Arguments )
            {
                builder.Append( $"{arg.Name}" );

                if( index < argCount - 1 )
                {
                    builder.Append( ", " );
                }

                index++;
            }

            builder.Append( ")" );
            builder.AppendLine( "**" );
        }
        else
        {
            builder.AppendLine( "**" );
            builder.AppendLine();
        }

        builder.AppendLine();
        builder.AppendLine( symbol.Description.Value );

        if( symbol.ArgumentCount == 0 )
        {
            return;
        }

        builder.AppendLine();

        /*
         * Arguments:
         *   - arg1 : description1 (if not empty)
         *   - arg2 : description2 (if not empty)
         * :
         * :
         */
        builder.AppendLine( "**Argument(s):**" );

        foreach( var arg in symbol.Arguments )
        {
            builder.Append( $"- {arg.Name}" );

            if( !string.IsNullOrEmpty( arg.Description ) )
            {
                builder.AppendLine( $" : {arg.Description}" );
            }
            else
            {
                builder.AppendLine();
            }
        }

        builder.AppendLine();
    }
}
