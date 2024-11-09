using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace KSPCompiler.ExternalSymbol.Tsv.Commons;

/// <summary>
/// A utility class that provides common data-independent logic in the TSV parsing process.
/// </summary>
internal static class TsvUtility
{
    /// <summary>
    /// A regular expression that matches a line comment in a TSV file. Begins with a '#'.
    /// </summary>
    public static readonly Regex RegexDefaultLineComment = new( @"^#.*" );

    /// <summary>
    /// Processes multi-line text line by line, splitting each line with a tab character and executing delegate parseBody.
    /// </summary>
    public static void ParseTsv( IEnumerable<string> lines, Regex? lineComment, Action<string[]> parseBody )
    {
        foreach( var x in lines )
        {
            if( string.IsNullOrWhiteSpace( x ) )
            {
                continue;
            }

            if( lineComment != null && lineComment.IsMatch( x ) )
            {
                continue;
            }

            parseBody( x.Split( '\t' ) );
        }
    }

    /// <summary>
    /// Processes multi-line text line by line, splitting each line with a tab character and executing delegate parseBody.
    /// </summary>
    /// <remarks>
    /// <see cref="RegexDefaultLineComment"/> will be used as the line comment regular expression.
    /// </remarks>
    public static void ParseTsv( IEnumerable<string> lines, Action<string[]> parseBody )
        => ParseTsv( lines, RegexDefaultLineComment, parseBody );

    /// <summary>
    /// Remove " from the beginning and end of the string
    /// </summary>
    /// <param name="line">Row data including column data</param>
    public static void RemoveQuoteCharacter( string[] line )
    {
        for( var i = 0; i < line.Length; i++ )
        {
            var v = line[ i ];

            if( v.StartsWith( "\"" ) && v.EndsWith( "\"" ) )
            {
                line[ i ] = v[ 1..^1 ];
            }
        }
    }

    /// <summary>
    /// Determine if the cell value is equal to "true"
    /// </summary>
    /// <param name="cellValue">A cell value</param>
    /// <returns>True if cellValue is "true" (Ignore case differences)</returns>
    public static bool ParseBoolean( string cellValue )
    {
        return cellValue.ToLower() == "true";
    }

    /// <summary>
    /// Groups consecutive columns starting from the specified index, and executes the provided
    /// delegate for each group of columns. Throws an IndexOutOfRangeException if the specified
    /// range exceeds the length of the input row.
    /// </summary>
    /// <param name="row">The row data to be processed</param>
    /// <param name="startIndex">The starting index for the column groups</param>
    /// <param name="consecutiveColumns">The size of each column group (number of consecutive columns)</param>
    /// <param name="parseBody">The delegate to be executed for each column group</param>
    /// <exception cref="IndexOutOfRangeException">Thrown when the specified range exceeds the length of the input row</exception>
    public static void ParseColumnGroups( string[] row, int startIndex, int consecutiveColumns, Action<IList<string>> parseBody )
    {
        for( var i = startIndex; i < row.Length; i += consecutiveColumns )
        {
            var values = new List<string>( consecutiveColumns );
            for( var j = 0; j < consecutiveColumns; j++ )
            {
                var index = i + j;

                if( index >= row.Length )
                {
                    throw new IndexOutOfRangeException( $"{row[ 0 ]} : Out of range (={index}). {nameof( row )}.Length = {row.Length}" );
                }

                values.Add( row[ index ] );
            }
            parseBody( values );
        }
    }
}
