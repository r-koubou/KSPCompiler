namespace KSPCompiler.Domain.Ast.Symbols.Extensions;

public static class DataTypeFlagExtension
{
    public static bool IsArray( this DataTypeFlag flag ) => ( flag & DataTypeFlag.Array ) != 0;
    public static bool AreArray( this DataTypeFlag a, DataTypeFlag b ) => a.IsArray() && b.IsArray();
}