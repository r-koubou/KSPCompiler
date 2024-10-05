using System.Text;

namespace KSPCompiler.ExternalSymbol.Tsv.Extensions;

internal static class StringBuilderExtension
{
    private const string Tab = "\t";
    private const string NewLine = "\n";

    public static StringBuilder AppendNewLine( this StringBuilder source, string value )
        => source.Append( value ).Append( NewLine );

    public static StringBuilder AppendNewLine( this StringBuilder source, params string[] values )
    {
        foreach( var x in values )
        {
            source.Append( x );
        }

        return source.Append( NewLine );
    }

    public static StringBuilder AppendNewLine( this StringBuilder source )
        => source.Append( NewLine );

    public static StringBuilder AppendTab( this StringBuilder source, string value )
        => source.Append( value ).Append( Tab );

    public static StringBuilder AppendTab( this StringBuilder source, params string[] values )
    {
        foreach( var x in values )
        {
            source.Append( x );
        }

        return source.Append( Tab );
    }

    public static StringBuilder AppendTab( this StringBuilder source )
        => source.Append( Tab );
}
