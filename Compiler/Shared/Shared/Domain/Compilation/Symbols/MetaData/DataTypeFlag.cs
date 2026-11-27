using System;

namespace KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;

/// <summary>
/// Representation of data type identifiers.
/// </summary>
[Flags]
public enum DataTypeFlag : uint
{
    None                        = 0,

    #region Type
    TypeInt                     = 1 << 0,
    TypeString                  = 1 << 1,
    TypeReal                    = 1 << 2,
    TypeBool                    = 1 << 3,
    TypeVoid                    = 1 << 4,
    TypePreprocessorSymbol      = 1 << 5,
    TypePgsId                   = 1 << 6,
    TypeMask                    = 0x7fffff,
    TypeNumerical               = TypeInt | TypeReal,
    TypeNonVariable             = TypePreprocessorSymbol | TypePgsId,
    #endregion

    #region Attribute
    AttributeArray              = 0x01000000,
    AttributeFallBack           = 0x80000000,
    AttributeMask               = 0xff000000,
    #endregion

    #region Alias
    TypeIntArray                = TypeInt    | AttributeArray,
    TypeStringArray             = TypeString | AttributeArray,
    TypeRealArray               = TypeReal   | AttributeArray,
    TypeBoolArray               = TypeBool   | AttributeArray,
    All                         = TypeInt | TypeString | TypeReal
                                  | TypeBool | TypeVoid | TypePreprocessorSymbol | TypePgsId
                                  | AttributeArray,
    FallBack                    = All | AttributeFallBack,
    #endregion
}
