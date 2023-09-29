using System;

namespace KSPCompiler.Domain.Symbols;

/// <summary>
/// Representation of data type identifiers.
/// </summary>
[Flags]
public enum DataTypeFlag
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