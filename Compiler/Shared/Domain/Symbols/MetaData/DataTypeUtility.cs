using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPCompiler.Shared.Domain.Symbols.MetaData;

public static class DataTypeUtility
{

    #region KSP Language data type tables
    private static readonly char[] KspTypeCharactersTable =
    [
        '$', '%', '~', '?', '@', '!'
    ];

    /// <summary>
    /// Get KSP data type characters list.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public static IReadOnlyCollection<char> KspTypeCharacters { get; }
        = KspTypeCharactersTable;

    /// <summary>
    /// Get KSP data type characters list.
    /// </summary>
    public static IReadOnlyCollection<string> KspTypeCharactersAsString { get; }
        = KspTypeCharactersTable.Select( x => x.ToString() ).ToList();

    /// <summary>
    /// Get KSP data type character mapped to data type flag.
    /// </summary>
    public static IReadOnlyDictionary<char, DataTypeFlag> KspTypeCharacterType { get; } = new Dictionary<char, DataTypeFlag>
    {
        { '$', DataTypeFlag.TypeInt },
        { '%', DataTypeFlag.TypeIntArray },
        { '~', DataTypeFlag.TypeReal },
        { '?', DataTypeFlag.TypeRealArray },
        { '@', DataTypeFlag.TypeString },
        { '!', DataTypeFlag.TypeStringArray }
    };
    #endregion

    /// <summary>
    /// Check if the given character matches the KSP data type character.
    /// </summary>
    /// <param name="c">A character to check.</param>
    /// <returns>True if the character is a KSP data type character, otherwise false.</returns>
    public static bool IsDataTypeCharacter( char c )
    {
        return KspTypeCharacters.Contains( c );
    }

    /// <summary>
    /// Check if the given character matches the KSP data type character.
    /// </summary>
    public static bool StartsWithDataTypeCharacter( string text )
    {
        return text.Length > 0 && IsDataTypeCharacter( text[ 0 ] );
    }

    /// <summary>
    /// Convert to data type from symbol name.
    /// </summary>
    /// <exception cref="ArgumentException">Unknown type</exception>
    public static DataTypeFlag GuessFromSymbolName( string symbolName )
    {
        if( string.IsNullOrEmpty( symbolName ) )
        {
            return DataTypeFlag.None;
        }

        var typePrefix = symbolName[ 0 ];

        if( KspRegExpConstants.NonTypePrefix.IsMatch( symbolName ) )
        {
            return DataTypeFlag.TypePreprocessorSymbol | DataTypeFlag.TypePgsId;
        }

        return typePrefix switch
        {
            //--------------------------------------------------------------------------
            // KSP Standard syntax
            //--------------------------------------------------------------------------
            '$' => DataTypeFlag.TypeInt,
            '%' => DataTypeFlag.TypeIntArray,
            '~' => DataTypeFlag.TypeReal,
            '?' => DataTypeFlag.TypeRealArray,
            '@' => DataTypeFlag.TypeString,
            '!' => DataTypeFlag.TypeStringArray,
            //--------------------------------------------------------------------------
            // For internal processing
            //--------------------------------------------------------------------------
            '*' => DataTypeFlag.All,
            _   => throw new ArgumentException( $"unknown ksp type : {typePrefix} ({nameof(symbolName)}={symbolName})" )
        };
    }

    /// <summary>
    /// Convert to data type from symbol name.
    /// </summary>
    /// <exception cref="ArgumentException">Unknown type</exception>
    public static DataTypeFlag GuessFromSymbolName( SymbolName symbolName )
        => GuessFromSymbolName( symbolName.Value );

    /// <summary>
    /// Convert to data type from string with separated '||' if multiple types.
    /// </summary>
    /// <returns>True if successful, otherwise false.</returns>
    public static bool TryGuessFromTypeString( string typeString, out DataTypeFlag result, string separator = "||" )
    {
        result = DataTypeFlag.None;

        foreach( var type in typeString.Split( separator ) )
        {
            result |= type switch
            {
                //--------------------------------------------------------------------------
                // KSP Standard syntax
                //--------------------------------------------------------------------------
                "$" => DataTypeFlag.TypeInt,
                "%" => DataTypeFlag.TypeIntArray,
                "~" => DataTypeFlag.TypeReal,
                "?" => DataTypeFlag.TypeRealArray,
                "@" => DataTypeFlag.TypeString,
                "!" => DataTypeFlag.TypeStringArray,
                //--------------------------------------------------------------------------
                // For internal processing
                //--------------------------------------------------------------------------
                "I"   => DataTypeFlag.TypeInt,
                "S"   => DataTypeFlag.TypeString,
                "R"   => DataTypeFlag.TypeReal,
                "B"   => DataTypeFlag.TypeBool,
                "I[]" => DataTypeFlag.TypeIntArray,
                "S[]" => DataTypeFlag.TypeStringArray,
                "R[]" => DataTypeFlag.TypeRealArray,
                "V"   => DataTypeFlag.TypeVoid,
                "P"   => DataTypeFlag.TypePreprocessorSymbol,
                "K"   => DataTypeFlag.TypePgsId,
                "*"   => DataTypeFlag.All,
                _     => DataTypeFlag.None
            };
        }

        return result != DataTypeFlag.None;
    }

    public static void GuessFromTypeString( string typeString, out DataTypeFlag dataTypeFlag, ref List<string> uiTypes, ref List<string> otherTypes, string separator = "||" )
    {
        if( !TryGuessFromTypeString( typeString, out dataTypeFlag ) )
        {
            dataTypeFlag = DataTypeFlag.None;
        }

        GuessFromOtherTypeString( typeString, ref uiTypes, ref otherTypes, separator );

    }

    /// <summary>
    /// Convert to data type from string with separated '||' if multiple types.
    /// </summary>
    /// <exception cref="ArgumentException">Unknown type</exception>
    public static DataTypeFlag GuessFromTypeString( string typeString, string separator = "||" )
    {
        if( !TryGuessFromTypeString( typeString, out var result, separator ) )
        {
            throw new ArgumentException( $"unknown type : {typeString}" );
        }

        return result;
    }

    public static void GuessFromOtherTypeString( string typeString, ref List<string> uiTypes, ref List<string> otherTypes, string separator = "||" )
    {
        GuessFromUITypeString( typeString, ref uiTypes, separator );
        GuessFromOtherTypeString( typeString, ref otherTypes, separator );
    }

    public static void GuessFromOtherTypeString( string typeString, ref List<string> otherTypes, string separator = "||" )
    {
        GuessFromOtherTypeStringImpl( typeString, ref otherTypes, x => !x.StartsWith( "ui_" ), separator );
    }

    public static void GuessFromUITypeString( string typeString, ref List<string> uiTypes, string separator = "||" )
    {
        GuessFromOtherTypeStringImpl( typeString, ref uiTypes, x => x.StartsWith( "ui_" ), separator );
    }

    private static void GuessFromOtherTypeStringImpl( string typeString, ref List<string> result, Predicate<string> condition, string separator = "||" )
    {
        result.Clear();

        foreach( var type in typeString.Split( separator ) )
        {
            // プリミティブ型はここでは対象外
            if( TryGuessFromTypeString( type, out _, separator ) )
            {
                continue;
            }

            if( condition( type ) )
            {
                result.Add( type );
            }
        }
    }

    #region Data type to string signature
    /// <summary>
    /// Convert to this compiler formated string from data type with separated '||' if multiple types.
    /// </summary>
    /// <exception cref="ArgumentException">Unknown type</exception>
    public static string ToString( DataTypeFlag typeFlag, string separator = "||" )
    {
        var resultTexts = new List<string>();

        if( typeFlag == DataTypeFlag.All )
        {
            return "*";
        }

        ToStringImpl( typeFlag, DataTypeFlag.TypeVoid,                  "V", resultTexts );
        ToStringImpl( typeFlag, DataTypeFlag.TypeInt,                   "I", resultTexts );
        ToStringImpl( typeFlag, DataTypeFlag.TypeString,                "S", resultTexts );
        ToStringImpl( typeFlag, DataTypeFlag.TypeReal,                  "R", resultTexts );
        ToStringImpl( typeFlag, DataTypeFlag.TypeBool,                  "B", resultTexts );
        ToStringImpl( typeFlag, DataTypeFlag.TypePreprocessorSymbol, "P", resultTexts );
        ToStringImpl( typeFlag, DataTypeFlag.TypePgsId,                 "K", resultTexts );

        if(resultTexts.Count == 0)
        {
            throw new ArgumentException( $"unknown type : {typeFlag}" );
        }

        var result = new StringBuilder();

        foreach( var (value, index) in resultTexts.Select( (value, index) => (value, index) ))
        {
            result.Append( value );

            if( index < resultTexts.Count - 1 )
            {
                result.Append( separator );
            }
        }

        return result.ToString();
    }

    private static void ToStringImpl( DataTypeFlag typeFlag, DataTypeFlag expectedType, string typeText, ICollection<string> outputTextTo )
    {
        string result;

        if( ( typeFlag & expectedType ) != 0 )
        {
            result = typeText;
        }
        else
        {
            return;
        }

        if( ( typeFlag & DataTypeFlag.AttributeArray ) != 0 )
        {
            result += "[]";
        }

        outputTextTo.Add( result );
    }

    public static void ToDataTypeString( StringBuilder result, ArgumentSymbol x, string separator = "||" )
    {
        var appendSeparator = false;

        if( x.DataType != DataTypeFlag.None )
        {
            appendSeparator = true;
            result.Append( ToString( x.DataType ) );
        }

        if( x.UITypeNames.Any() )
        {
            if( appendSeparator )
            {
                result.Append( separator );
            }

            appendSeparator = x.UITypeNames.Any();
            ToDataTypeString( result, x.UITypeNames );
        }

        if( x.OtherTypeNames.Any() )
        {
            if( appendSeparator )
            {
                result.Append( separator );
            }

            ToDataTypeString( result, x.OtherTypeNames );
        }
    }

    private static void ToDataTypeString( StringBuilder result, IReadOnlyList<string> types, string separator = "||" )
    {
        var count = types.Count;

        for( var k = 0; k < count; k++ )
        {
            result.Append( types[ k ] );

            if( k != count - 1 )
            {
                result.Append( separator );
            }
        }
    }
    #endregion ~Data type to string signature
}
