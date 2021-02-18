namespace KSPCompiler.Domain.Ast.Constants
{
    /// <summary>
    /// Representation of attributes associated with data types.
    /// </summary>
    [System.Flags]
    public enum DataTypeAttribute
    {
        None            = 0,
        Array           = 1 << 0,
    }

    public static class DataTypeAttributeExtension
    {
        public static bool IsArray( this DataTypeAttribute attribute ) => ( attribute & DataTypeAttribute.Array ) != 0;
        public static bool AreArray( this DataTypeAttribute a, DataTypeAttribute b ) => a.IsArray() && b.IsArray();
    }
}
