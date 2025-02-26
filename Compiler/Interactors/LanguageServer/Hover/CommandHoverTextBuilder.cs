using System.Text;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;

namespace KSPCompiler.Interactors.LanguageServer.Hover;

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
         *   - arg1
         *     - type1
         *  - arg2
         *    - type2
         * :
         * :
         */
        builder.AppendLine( "**Arguments:**" );

        foreach( var arg in symbol.Arguments )
        {
            var argType = arg.DataType.ToMessageString();

            builder.AppendLine( $"- {arg.Name}" );

            if( arg.UITypeNames.Count > 0 )
            {
                builder.AppendLine( $"  - {string.Join( ", ", arg.UITypeNames )}" );
            }
            else
            {
                builder.AppendLine( $"  - {argType}" );
            }
        }

        return builder.ToString();
    }
}
