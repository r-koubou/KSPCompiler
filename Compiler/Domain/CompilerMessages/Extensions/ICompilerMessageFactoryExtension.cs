using System;

namespace KSPCompiler.Domain.CompilerMessages.Extensions;

/// <summary>
/// Extension for <see cref="ICompilerMessageFactory"/>
/// </summary>
// ReSharper disable once InconsistentNaming
public static class ICompilerMessageFactoryExtension
{
    #region Alias
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

    #endregion ~Alias


    #region Alias and Format Support

    /// <summary>
    /// Alias for <see cref="ICompilerMessageFactory.Create(CompilerMessageLevel, string, int, int, Exception)"/>.
    /// </summary>
    /// <remarks>
    /// Format support version of <see cref="Create(KSPCompiler.Domain.CompilerMessages.ICompilerMessageFactory,KSPCompiler.Domain.CompilerMessages.CompilerMessageLevel,string)"/>.
    /// </remarks>
    /// <seealso cref="string.Format(System.IFormatProvider,string,object)"/>
    public static CompilerMessage Create( this ICompilerMessageFactory self, CompilerMessageLevel level, string message, params object[] parameters )
        => self.Create( level, string.Format( message, parameters ) );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageFactory.Create(CompilerMessageLevel, string, int, int, Exception)"/> with <see cref="CompilerMessageLevel.Info"/>.
    /// </summary>
    /// <remarks>
    /// Format support version of <see cref="Info(KSPCompiler.Domain.CompilerMessages.ICompilerMessageFactory,string)"/>
    /// </remarks>
    /// <seealso cref="string.Format(System.IFormatProvider,string,object)"/>
    public static CompilerMessage Info( this ICompilerMessageFactory self, string message, params object[] parameters )
        => self.Create( CompilerMessageLevel.Info, string.Format( message, parameters ) );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageFactory.Create(CompilerMessageLevel, string, int, int, Exception)"/> with <see cref="CompilerMessageLevel.Warning"/>.
    /// </summary>
    /// <remarks>
    /// Format support version of <see cref="Info(KSPCompiler.Domain.CompilerMessages.ICompilerMessageFactory,string)"/>
    /// </remarks>
    /// <seealso cref="string.Format(System.IFormatProvider,string,object)"/>
    public static CompilerMessage Warning( this ICompilerMessageFactory self, string message, params object[] parameters )
        => self.Create( CompilerMessageLevel.Warning, string.Format( message, parameters ) );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageFactory.Create(CompilerMessageLevel, string, int, int, Exception)"/> with <see cref="CompilerMessageLevel.Error"/>.
    /// </summary>
    /// <remarks>
    /// Format support version of <see cref="Info(KSPCompiler.Domain.CompilerMessages.ICompilerMessageFactory,string)"/>
    /// </remarks>
    /// <seealso cref="string.Format(System.IFormatProvider,string,object)"/>
    public static CompilerMessage Error( this ICompilerMessageFactory self, string message, params object[] parameters )
        => self.Create( CompilerMessageLevel.Error, string.Format( message, parameters ) );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageFactory.Create(CompilerMessageLevel, string, int, int, Exception)"/> with <see cref="CompilerMessageLevel.Fatal"/>.
    /// </summary>
    /// <remarks>
    /// Format support version of <see cref="Info(KSPCompiler.Domain.CompilerMessages.ICompilerMessageFactory,string)"/>
    /// </remarks>
    /// <seealso cref="string.Format(System.IFormatProvider,string,object)"/>
    public static CompilerMessage Fatal( this ICompilerMessageFactory self, string message, params object[] parameters )
        => self.Create( CompilerMessageLevel.Fatal, string.Format( message, parameters ) );

    #endregion

}
