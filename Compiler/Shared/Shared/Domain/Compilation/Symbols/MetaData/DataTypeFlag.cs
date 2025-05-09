﻿using System;

namespace KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;

/// <summary>
/// Representation of data type identifiers.
/// </summary>
[Flags]
public enum DataTypeFlag : uint
{
    None                        = 0,
    All                         = 0xffffffff,
    FallBack                    = All,

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
    AttributeMask               = 0xff000000,
    #endregion

    #region Alias
    TypeIntArray                = TypeInt    | AttributeArray,
    TypeStringArray             = TypeString | AttributeArray,
    TypeRealArray               = TypeReal   | AttributeArray,
    TypeBoolArray               = TypeBool   | AttributeArray,
    #endregion
}
