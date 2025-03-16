using System.Text;

using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.LanguageServer.UseCase.Hover;

public class CommandHoverTextBuilder : IHoverTextBuilder<CommandSymbol>
{
    public string BuildHoverText( CommandSymbol symbol )
    {
        var builder = new StringBuilder( 64 );

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
            return builder.ToString();
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

        return builder.ToString();
    }
}
