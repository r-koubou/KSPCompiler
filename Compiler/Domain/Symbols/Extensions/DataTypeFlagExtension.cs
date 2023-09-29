using System.Diagnostics.CodeAnalysis;

namespace KSPCompiler.Domain.Symbols.Extensions;

[SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" )]
public static class DataTypeFlagExtension
{
    public static bool IsInt( this DataTypeFlag flag ) => ( flag & DataTypeFlag.Int ) != 0;
    public static bool AreInt( this DataTypeFlag a, DataTypeFlag b ) => a.IsInt() && b.IsInt();

    public static bool IsReal( this DataTypeFlag typeFlag ) => ( typeFlag & DataTypeFlag.Real ) != 0;
    public static bool AreReal( this DataTypeFlag a, DataTypeFlag b ) => a.IsReal() && b.IsReal();

    public static bool IsString( this DataTypeFlag typeFlag ) => ( typeFlag & DataTypeFlag.String ) != 0;
    public static bool AreString( this DataTypeFlag a, DataTypeFlag b ) => a.IsString() && b.IsString();

    public static bool IsBoolean( this DataTypeFlag typeFlag ) => ( typeFlag & DataTypeFlag.Bool ) != 0;
    public static bool AreBoolean( this DataTypeFlag a, DataTypeFlag b ) => a.IsBoolean() && b.IsBoolean();

    public static bool IsVoid( this DataTypeFlag typeFlag ) => ( typeFlag & DataTypeFlag.Void ) != 0;
    public static bool AreVoid( this DataTypeFlag a, DataTypeFlag b ) => a.IsVoid() && b.IsVoid();

    public static bool IsNumerical( this DataTypeFlag typeFlag ) => ( typeFlag & DataTypeFlag.Numerical ) != 0;
    public static bool AreNumerical( this DataTypeFlag a, DataTypeFlag b ) => a.IsNumerical() && b.IsNumerical();

    public static bool IsMultiType( this DataTypeFlag typeFlag ) => ( typeFlag & DataTypeFlag.Multiple ) != 0;
    public static bool AreMultiType( this DataTypeFlag a, DataTypeFlag b ) => a.IsMultiType() && b.IsMultiType();

    public static bool IsNonVariableType( this DataTypeFlag typeFlag ) => ( typeFlag & DataTypeFlag.NonVariable ) != 0;
    public static bool AreNonVariableType( this DataTypeFlag a, DataTypeFlag b ) => a.IsNonVariableType() && b.IsNonVariableType();
}
