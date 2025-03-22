using System.Collections.Generic;
using System.Linq;
using System.Text;

using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData.Extensions;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics.Extensions;

public static class CommandArgumentSymbolListExtension
{
    public static string ToIncompatibleMessage( this IReadOnlyCollection<CommandSymbol> self )
    {
        var stringBuilder = new StringBuilder();

        var i = 0;
        var length = self.Count;
        var commandName = self.First().Name.Value;

        foreach( var command in self )
        {
            stringBuilder.Append( command.Arguments.ToIncompatibleMessage( commandName ) );

            if( i < length - 1 )
            {
                stringBuilder.Append( " or " );
            }
            i++;
        }

        return stringBuilder.ToString();

    }

    public static string ToIncompatibleMessage( this CommandArgumentSymbolList self, string commandName )
    {
        var stringBuilder = new StringBuilder();

        var i = 0;
        var length = self.Count;

        stringBuilder.Append( commandName );
        stringBuilder.Append( '(' );

        foreach( var arg in self )
        {
            stringBuilder.Append( arg.DataType.ToMessageString() );

            if( i < length - 1 )
            {
                stringBuilder.Append( ", " );
            }
            i++;
        }

        stringBuilder.Append( ')' );

        return stringBuilder.ToString();
    }

    public static string ToIncompatibleMessage( this IReadOnlyCollection<AstExpressionNode> self, string commandName )
    {
        var stringBuilder = new StringBuilder();

        var i = 0;
        var length = self.Count();

        stringBuilder.Append( commandName );
        stringBuilder.Append( '(' );

        foreach( var arg in self )
        {
            stringBuilder.Append( arg.TypeFlag.ToMessageString() );

            if( i < length - 1 )
            {
                stringBuilder.Append( ", " );
            }
            i++;
        }

        stringBuilder.Append( ')' );

        return stringBuilder.ToString();
    }
}
