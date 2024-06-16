using System;

namespace KSPCompiler.Domain.CompilerMessages.Extensions;

/// <summary>
/// Extension for <see cref="ICompilerMessageFactory"/>
/// </summary>
// ReSharper disable once InconsistentNaming
public static class ICompilerMessageFactoryExtension
{
    /// <summary>
    /// Alias for <see cref="ICompilerMessageFactory.Create(CompilerMessageLevel, string, int, int, Exception)"/>.
    /// </summary>
    public static CompilerMessage Create( this ICompilerMessageFactory self, CompilerMessageLevel level, string message )
        => self.Create( level, message );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageFactory.Create(CompilerMessageLevel, string, int, int, Exception)"/> with <see cref="CompilerMessageLevel.Info"/>.
    /// </summary>
    public static CompilerMessage Info( this ICompilerMessageFactory self, string message )
        => self.Create( CompilerMessageLevel.Info, message );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageFactory.Create(CompilerMessageLevel, string, int, int, Exception)"/> with <see cref="CompilerMessageLevel.Warning"/>.
    /// </summary>
    public static CompilerMessage Warning( this ICompilerMessageFactory self, string message )
        => self.Create( CompilerMessageLevel.Warning, message );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageFactory.Create(CompilerMessageLevel, string, int, int, Exception)"/> with <see cref="CompilerMessageLevel.Error"/>.
    /// </summary>
    public static CompilerMessage Error( this ICompilerMessageFactory self, string message )
        => self.Create( CompilerMessageLevel.Error, message );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageFactory.Create(CompilerMessageLevel, string, int, int, Exception)"/> with <see cref="CompilerMessageLevel.Fatal"/>.
    /// </summary>
    public static CompilerMessage Fatal( this ICompilerMessageFactory self, string message )
        => self.Create( CompilerMessageLevel.Fatal, message );
}
