namespace KSPCompiler.Features.Compilation.Domain;

public static class KspLanguageLimitations
{
    /// <summary>
    /// Maximum number of lines that can be written in the init callback.
    /// </summary>
    public const int OverflowLines = 4950;

    /// <summary>
    /// Maximum number of array elements that can be declared.
    /// </summary>
    public const int MaxArraySize = 1000000;
}
