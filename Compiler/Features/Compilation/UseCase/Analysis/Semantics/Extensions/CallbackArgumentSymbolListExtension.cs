using System.Text;

using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData.Extensions;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics.Extensions;

public static class CallbackArgumentSymbolListExtension
{
    public static string ToIncompatibleMessage( this CallbackArgumentSymbolList self, string callbackName )
    {
        var stringBuilder = new StringBuilder();

        var i = 0;
        var length = self.Count;

        stringBuilder.Append( callbackName );
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
}
