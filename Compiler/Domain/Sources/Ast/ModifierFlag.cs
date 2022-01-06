﻿using System;

namespace KSPCompiler.Domain.Ast
{
    /// <summary>
    /// Symbol modifier definition.
    /// </summary>
    [Flags]
    public enum ModifierFlag
    {
        None = 0,
        Const = 1 << 0,
        Polyphonic = 1 << 1,
        // ReSharper disable once InconsistentNaming
        UI = 1 << 2,
    }
}
