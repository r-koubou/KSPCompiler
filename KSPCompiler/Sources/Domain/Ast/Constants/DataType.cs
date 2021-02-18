namespace KSPCompiler.Domain.Ast.Constants
{
    /// <summary>
    /// Representation of data type identifiers.
    /// </summary>
    [System.Flags]
    public enum DataType
    {
        None                    = 0,
        Int                     = 1 << 0,
        String                  = 1 << 1,
        Real                    = 1 << 2,
        Bool                    = 1 << 3,
        Void                    = 1 << 4,
        KspPreprocessorSymbol   = 1 << 5,
        PgsId                   = 1 << 6,
        Numerical               = Int | Real,
        NonVariable             = KspPreprocessorSymbol | PgsId,
        Multiple                = 0x7fffffff & ~NonVariable
    }

    public static class DataTypeExtension
    {
        public static bool IsInt( this DataType type ) => ( type & DataType.Int ) != 0;
        public static bool AreInt( this DataType a, DataType b ) => a.IsInt() && b.IsInt();

        public static bool IsReal( this DataType type ) => ( type & DataType.Real ) != 0;
        public static bool AreReal( this DataType a, DataType b ) => a.IsReal() && b.IsReal();

        public static bool IsString( this DataType type ) => ( type & DataType.String ) != 0;
        public static bool AreString( this DataType a, DataType b ) => a.IsString() && b.IsString();

        public static bool IsBoolean( this DataType type ) => ( type & DataType.Bool ) != 0;
        public static bool AreBoolean( this DataType a, DataType b ) => a.IsBoolean() && b.IsBoolean();

        public static bool IsVoid( this DataType type ) => ( type & DataType.Void ) != 0;
        public static bool AreVoid( this DataType a, DataType b ) => a.IsVoid() && b.IsVoid();

        public static bool IsNumerical( this DataType type ) => ( type & DataType.Numerical ) != 0;
        public static bool AreNumerical( this DataType a, DataType b ) => a.IsNumerical() && b.IsNumerical();

        public static bool IsMultiType( this DataType type ) => ( type & DataType.Multiple ) != 0;
        public static bool AreMultiType( this DataType a, DataType b ) => a.IsMultiType() && b.IsMultiType();

        public static bool IsNonVariableType( this DataType type ) => ( type & DataType.NonVariable ) != 0;
        public static bool AreNonVariableType( this DataType a, DataType b ) => a.IsNonVariableType() && b.IsNonVariableType();

    }
}
