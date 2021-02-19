namespace KSPCompiler.Domain.Ast.Constants
{
    /// <summary>
    /// Representation of attributes associated with data types.
    /// </summary>
    [System.Flags]
    public enum DataTypeFlag
    {
        None = 0,
        Array = 1 << 0,
    }

    public static class DataTypeAttributeExtension
    {
        public static bool IsArray( this DataTypeFlag flag ) => ( flag & DataTypeFlag.Array ) != 0;
        public static bool AreArray( this DataTypeFlag a, DataTypeFlag b ) => a.IsArray() && b.IsArray();
    }
}
