using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPCompiler.Domain.Symbols.MetaData;

public static class DataTypeUtility
{
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
            '*' => DataTypeFlag.MultipleType,
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
                "*"   => DataTypeFlag.MultipleType,
                _     => DataTypeFlag.None
            };
        }

        return result != DataTypeFlag.None;
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

    /// <summary>
    /// Convert to this compiler formated string from data type with separated '||' if multiple types.
    /// </summary>
    /// <exception cref="ArgumentException">Unknown type</exception>
    public static string ToString( DataTypeFlag typeFlag, string separator = "||" )
    {
        var resultTexts = new List<string>();

        if( typeFlag == DataTypeFlag.MultipleType )
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
}
