using System;

namespace KSPCompiler.Commons;

[Serializable]
public sealed class Unit
{
    public static readonly Unit Default = new();

    private Unit() {}
}
