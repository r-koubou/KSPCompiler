using System.Diagnostics.CodeAnalysis;

namespace KSPCompiler.Domain.Symbols.MetaData.Extensions;

[SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" )]
public static class DataTypeFlagExtension
{
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

    public static bool IsVoid( this DataTypeFlag flag )
        => flag.HasFlag( DataTypeFlag.TypeVoid );

    public static bool AreVoid( this DataTypeFlag a, DataTypeFlag b )
        => a.IsVoid() && b.IsVoid();

    public static bool IsNumerical( this DataTypeFlag flag )
        => flag.HasFlag( DataTypeFlag.TypeNumerical );

    public static bool AreNumerical( this DataTypeFlag a, DataTypeFlag b )
        => a.IsNumerical() && b.IsNumerical();

    public static bool IsMultiType( this DataTypeFlag flag )
        => flag.HasFlag( DataTypeFlag.MultipleType );

    public static bool AreMultiType( this DataTypeFlag a, DataTypeFlag b )
        => a.IsMultiType() && b.IsMultiType();

    public static bool IsNonVariableType( this DataTypeFlag flag )
        => flag.HasFlag( DataTypeFlag.None );

    public static bool AreNonVariableType( this DataTypeFlag a, DataTypeFlag b )
        => a.IsNonVariableType() && b.IsNonVariableType();

    public static bool IsArray( this DataTypeFlag flag )
        => flag.HasFlag( DataTypeFlag.AttributeArray );

    public static bool AreArray( this DataTypeFlag a, DataTypeFlag b )
        => a.IsArray() && b.IsArray();
}
