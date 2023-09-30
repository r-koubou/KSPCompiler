﻿using System;

namespace KSPCompiler.Domain.Symbols.MetaData;

/// <summary>
/// Representation of data type identifiers.
/// </summary>
[Flags]
public enum DataTypeFlag : uint
{
    None                        = 0,
    TypeInt                     = 1 << 0,
    TypeString                  = 1 << 1,
    TypeReal                    = 1 << 2,
    TypeBool                    = 1 << 3,
    TypeVoid                    = 1 << 4,
    TypeKspPreprocessorSymbol   = 1 << 5,
    TypePgsId                   = 1 << 6,
    TypeNumerical               = TypeInt | TypeReal,
    TypeNonVariable             = TypeKspPreprocessorSymbol | TypePgsId,
    MultipleType                = 0x00ffffff & ~TypeNonVariable,

    AttributeArray              = 0x01000000,
    All                         = 0xffffffff,
}