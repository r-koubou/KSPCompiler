using System.Diagnostics.CodeAnalysis;

namespace KSPCompiler.Domain.Symbols.Extensions;

[SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" )]
public static class DataTypeAttributeFlagExtension
{
    public static bool IsArray( this DataTypeAttributeFlag attributeFlag ) => ( attributeFlag & DataTypeAttributeFlag.Array ) != 0;
    public static bool AreArray( this DataTypeAttributeFlag a, DataTypeAttributeFlag b ) => a.IsArray() && b.IsArray();
}