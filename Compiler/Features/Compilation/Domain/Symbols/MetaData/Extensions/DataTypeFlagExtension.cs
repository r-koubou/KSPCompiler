using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace KSPCompiler.Features.Compilation.Domain.Symbols.MetaData.Extensions;

[SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" )]
public static class DataTypeFlagExtension
{
    public static bool IsFallBack( this DataTypeFlag flag )
        => flag == DataTypeFlag.FallBack;

    public static bool AreFallBack( this DataTypeFlag a, DataTypeFlag b )
        => a.IsFallBack() && b.IsFallBack();

    public static bool OrFallBack( this DataTypeFlag a, DataTypeFlag b )
        => a.IsFallBack() || b.IsFallBack();

    public static bool IsInt( this DataTypeFlag flag )
        => flag.HasFlag( DataTypeFlag.TypeInt );

    public static bool AreInt( this DataTypeFlag a, DataTypeFlag b )
        => a.IsInt() && b.IsInt();

    public static bool IsReal( this DataTypeFlag flag ) => flag.HasFlag( DataTypeFlag.TypeReal );

    public static bool AreReal( this DataTypeFlag a, DataTypeFlag b )
        => a.IsReal() && b.IsReal();

    public static bool IsString( this DataTypeFlag flag )
        => flag.HasFlag( DataTypeFlag.TypeString );

    public static bool AreString( this DataTypeFlag a, DataTypeFlag b )
        => a.IsString() && b.IsString();

    public static bool IsBoolean( this DataTypeFlag flag )
        => flag.HasFlag( DataTypeFlag.TypeBool );

    public static bool AreBoolean( this DataTypeFlag a, DataTypeFlag b )
        => a.IsBoolean() && b.IsBoolean();

    public static bool IsPreprocessor( this DataTypeFlag flag )
        => flag.HasFlag( DataTypeFlag.TypePreprocessorSymbol );

    public static bool IsPgsId( this DataTypeFlag flag )
        => flag.HasFlag( DataTypeFlag.TypePgsId );

    public static bool IsVoid( this DataTypeFlag flag )
        => flag.HasFlag( DataTypeFlag.TypeVoid );

    public static bool AreVoid( this DataTypeFlag a, DataTypeFlag b )
        => a.IsVoid() && b.IsVoid();

    public static bool IsNumerical( this DataTypeFlag flag )
        => ( flag & DataTypeFlag.TypeNumerical ) != 0;

    public static bool AreNumerical( this DataTypeFlag a, DataTypeFlag b )
        => a.IsNumerical() && b.IsNumerical();

    public static bool IsMultiType( this DataTypeFlag flag )
        => flag.HasFlag( DataTypeFlag.All );

    public static bool AreMultiType( this DataTypeFlag a, DataTypeFlag b )
        => a.IsMultiType() && b.IsMultiType();

    public static bool IsNonVariableType( this DataTypeFlag flag )
        => flag.HasFlag( DataTypeFlag.None );

    public static bool AreNonVariableType( this DataTypeFlag a, DataTypeFlag b )
        => a.IsNonVariableType() && b.IsNonVariableType();

    public static bool HasAttribute( this DataTypeFlag flag )
        => ( flag & DataTypeFlag.AttributeMask ) != 0;

    public static bool IsArray( this DataTypeFlag flag )
        => flag.HasFlag( DataTypeFlag.AttributeArray );

    public static bool AreArray( this DataTypeFlag a, DataTypeFlag b )
        => a.IsArray() && b.IsArray();

    public static DataTypeFlag TypeMasked( this DataTypeFlag flag )
        => flag & DataTypeFlag.TypeMask;

    public static DataTypeFlag MatchedType( this DataTypeFlag flag, DataTypeFlag other )
        => flag.TypeMasked() & other.TypeMasked();

    private static readonly IReadOnlyDictionary<DataTypeFlag, string> MessageTextMap = new Dictionary<DataTypeFlag, string>
    {
        { DataTypeFlag.None, "unknown" },
        { DataTypeFlag.TypeInt, "integer" },
        { DataTypeFlag.TypeString, "string" },
        { DataTypeFlag.TypeReal, "real" },
        { DataTypeFlag.TypeBool, "boolean" },
        { DataTypeFlag.TypeVoid, "void" },
        { DataTypeFlag.TypePreprocessorSymbol, "KSP preprocessor symbol" },
        { DataTypeFlag.TypePgsId, "Pgs ID" }
    };

    public static string ToMessageString( this DataTypeFlag flag )
    {
        var typeMasked = flag.TypeMasked();
        var isArray = flag.IsArray();

        if( MessageTextMap.TryGetValue( flag, out var messageText ) )
        {
            return messageText;
        }

        var result = "";
        var foundCount = 0;

        foreach( var (k,v) in MessageTextMap )
        {
            var type = typeMasked & k;
            if( type == 0 )
            {
                continue;
            }

            if( foundCount > 0 )
            {
                result += " or ";
            }

            result += $"{v}{( isArray ? "[]" : string.Empty )}";
            foundCount++;
        }

        return result;
    }
}
