using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData.Extensions;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics.Extensions;

public static class CommandArgumentSymbolListExtension
{
    public static string ToIncompatibleMessage( this IReadOnlyCollection<CommandSymbol> self )
    {
        var i = 0;
        var length = self.Count;
        var commandName = self.First().Name.Value;
        var stringHandler = new DefaultInterpolatedStringHandler( 0, 0 );

        foreach( var command in self )
        {
            stringHandler.AppendFormatted( command.Arguments.ToIncompatibleMessage( commandName ) );

            if( i < length - 1 )
            {
                stringHandler.AppendLiteral( " or " );
            }
            i++;
        }

        return stringHandler.ToStringAndClear();

    }

    public static string ToIncompatibleMessage( this CommandArgumentSymbolList self, string commandName )
    {
        var i = 0;
        var length = self.Count;
        var stringHandler = new DefaultInterpolatedStringHandler( 0, 0 );

        stringHandler.AppendFormatted( commandName );
        stringHandler.AppendLiteral( "(" );

        foreach( var arg in self )
        {
            stringHandler.AppendFormatted( arg.DataType.ToMessageString() );

            if( i < length - 1 )
            {
                stringHandler.AppendLiteral( ", " );
            }
            i++;
        }

        stringHandler.AppendLiteral( ")" );

        return stringHandler.ToStringAndClear();
    }

    public static string ToIncompatibleMessage( this IReadOnlyCollection<AstExpressionNode> self, string commandName )
    {
        var i = 0;
        var length = self.Count;
        var stringHandler = new DefaultInterpolatedStringHandler(
            literalLength: "()".Length + ", ".Length * ( length - 1 ),
            formattedCount: length + 1
        );

        stringHandler.AppendFormatted( commandName );
        stringHandler.AppendLiteral( "(" );

        foreach( var arg in self )
        {
            stringHandler.AppendFormatted( arg.TypeFlag.ToMessageString() );

            if( i < length - 1 )
            {
                stringHandler.AppendLiteral( ", " );
            }
            i++;
        }

        stringHandler.AppendLiteral( ")" );

        return stringHandler.ToStringAndClear();
    }
}
