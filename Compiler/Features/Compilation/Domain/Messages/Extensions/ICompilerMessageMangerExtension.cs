using System;

using KSPCompiler.Shared.Text;

namespace KSPCompiler.Features.Compilation.Domain.Messages.Extensions;

/// <summary>
/// Extension for <see cref="ICompilerMessageManger"/>
/// </summary>
// ReSharper disable once InconsistentNaming
public static class ICompilerMessageMangerExtension
{
    #region Alias with specific line and column

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/>.
    /// </summary>
    private static void Append( this ICompilerMessageManger self, CompilerMessageLevel level, int lineNo, int column, string message, Exception? exception = null )
        => self.Append( self.MessageFactory.Create( level, lineNo, column, message, exception ) );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/> with <see cref="CompilerMessageLevel.Info"/>.
    /// </summary>
    public static void Info( this ICompilerMessageManger self, int lineNo, int column, string message, Exception? exception = null )
        => Append( self, CompilerMessageLevel.Info, lineNo, column, message, exception );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/> with <see cref="CompilerMessageLevel.Warning"/>.
    /// </summary>
    public static void Warning( this ICompilerMessageManger self, int lineNo, int column, string message, Exception? exception = null )
        => Append( self, CompilerMessageLevel.Warning, lineNo, column, message, exception );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/> with <see cref="CompilerMessageLevel.Error"/>.
    /// </summary>
    public static void Error( this ICompilerMessageManger self, int lineNo, int column, string message, Exception? exception = null )
        => Append( self, CompilerMessageLevel.Error, lineNo, column, message, exception );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/> with <see cref="CompilerMessageLevel.Fatal"/>.
    /// </summary>
    public static void Fatal( this ICompilerMessageManger self, int lineNo, int column, string message, Exception? exception = null )
        => Append( self, CompilerMessageLevel.Fatal, lineNo, column, message, exception );

    #endregion ~Alias with specific line and column

    #region Alias with specific position

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/>.
    /// </summary>
    private static void Append( this ICompilerMessageManger self, CompilerMessageLevel level, Position position, string message, Exception? exception = null )
        => self.Append( self.MessageFactory.Create( level, position, message, exception ) );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/> with <see cref="CompilerMessageLevel.Info"/>.
    /// </summary>
    public static void Debug( this ICompilerMessageManger self, Position position, string message, Exception? exception = null )
        => Append( self, CompilerMessageLevel.Debug, position, message, exception );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/> with <see cref="CompilerMessageLevel.Info"/>.
    /// </summary>
    public static void Info( this ICompilerMessageManger self, Position position, string message, Exception? exception = null )
        => Append( self, CompilerMessageLevel.Info, position, message, exception );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/> with <see cref="CompilerMessageLevel.Warning"/>.
    /// </summary>
    public static void Warning( this ICompilerMessageManger self, Position position, string message, Exception? exception = null )
        => Append( self, CompilerMessageLevel.Warning, position, message, exception );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/> with <see cref="CompilerMessageLevel.Error"/>.
    /// </summary>
    public static void Error( this ICompilerMessageManger self, Position position, string message, Exception? exception = null )
        => Append( self, CompilerMessageLevel.Error, position, message, exception );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/> with <see cref="CompilerMessageLevel.Fatal"/>.
    /// </summary>
    public static void Fatal( this ICompilerMessageManger self, Position position, string message, Exception? exception = null )
        => Append( self, CompilerMessageLevel.Fatal, position, message, exception );

    #endregion ~Alias with specific position

    #region Alias with specific line and column / Format Support

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/> with <see cref="CompilerMessageLevel.Info"/>.
    /// </summary>
    /// <remarks>
    /// Format support version of <see cref="Info(KSPCompiler.Features.Compilation.Domain.Messages.ICompilerMessageManger,int,int,string,System.Exception?)"/>.
    /// </remarks>
    /// <seealso cref="string.Format(System.IFormatProvider,string,object)"/>
    public static void Info( this ICompilerMessageManger self, int lineNo, int column, string format, params object[] parameters )
        => Append( self, CompilerMessageLevel.Info, lineNo, column, string.Format( format, parameters ) );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/> with <see cref="CompilerMessageLevel.Warning"/>.
    /// </summary>
    /// <remarks>
    /// Format support version of <see cref="Warning(KSPCompiler.Features.Compilation.Domain.Messages.ICompilerMessageManger,int,int,string,System.Exception?)"/>.
    /// </remarks>
    /// <seealso cref="string.Format(System.IFormatProvider,string,object)"/>
    public static void Warning( this ICompilerMessageManger self, int lineNo, int column, string format, params object[] parameters )
        => Append( self, CompilerMessageLevel.Warning, lineNo, column, string.Format( format, parameters ) );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/> with <see cref="CompilerMessageLevel.Error"/>.
    /// </summary>
    /// <remarks>
    /// Format support version of <see cref="Error(KSPCompiler.Features.Compilation.Domain.Messages.ICompilerMessageManger,int,int,string,System.Exception?)"/>.
    /// </remarks>
    /// <seealso cref="string.Format(System.IFormatProvider,string,object)"/>
    public static void Error( this ICompilerMessageManger self, int lineNo, int column, string format, params object[] parameters )
        => Append( self, CompilerMessageLevel.Error, lineNo, column, string.Format( format, parameters ) );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/> with <see cref="CompilerMessageLevel.Fatal"/>.
    /// </summary>
    /// <remarks>
    /// Format support version of <see cref="Fatal(KSPCompiler.Features.Compilation.Domain.Messages.ICompilerMessageManger,int,int,string,System.Exception?)"/>.
    /// </remarks>
    /// <seealso cref="string.Format(System.IFormatProvider,string,object)"/>
    public static void Fatal( this ICompilerMessageManger self, int lineNo, int column, string format, params object[] parameters )
        => Append( self, CompilerMessageLevel.Fatal, lineNo, column, string.Format( format, parameters ) );

    #endregion ~Alias with specific line and column / Format Support

    #region Alias with specific position / Format Support

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/> with <see cref="CompilerMessageLevel.Info"/>.
    /// </summary>
    /// <remarks>
    /// Format support version of <see cref="Info(KSPCompiler.Features.Compilation.Domain.Messages.ICompilerMessageManger,int,int,string,System.Exception?)"/>.
    /// </remarks>
    /// <seealso cref="string.Format(System.IFormatProvider,string,object)"/>
    public static void Info( this ICompilerMessageManger self, Position position, string format, params object[] parameters )
        => Append( self, CompilerMessageLevel.Info, position, string.Format( format, parameters ) );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/> with <see cref="CompilerMessageLevel.Warning"/>.
    /// </summary>
    /// <remarks>
    /// Format support version of <see cref="Warning(KSPCompiler.Features.Compilation.Domain.Messages.ICompilerMessageManger,int,int,string,System.Exception?)"/>.
    /// </remarks>
    /// <seealso cref="string.Format(System.IFormatProvider,string,object)"/>
    public static void Warning( this ICompilerMessageManger self, Position position, string format, params object[] parameters )
        => Append( self, CompilerMessageLevel.Warning, position, string.Format( format, parameters ) );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/> with <see cref="CompilerMessageLevel.Error"/>.
    /// </summary>
    /// <remarks>
    /// Format support version of <see cref="Error(KSPCompiler.Features.Compilation.Domain.Messages.ICompilerMessageManger,int,int,string,System.Exception?)"/>.
    /// </remarks>
    /// <seealso cref="string.Format(System.IFormatProvider,string,object)"/>
    public static void Error( this ICompilerMessageManger self, Position position, string format, params object[] parameters )
        => Append( self, CompilerMessageLevel.Error, position, string.Format( format, parameters ) );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/> with <see cref="CompilerMessageLevel.Fatal"/>.
    /// </summary>
    /// <remarks>
    /// Format support version of <see cref="Fatal(KSPCompiler.Features.Compilation.Domain.Messages.ICompilerMessageManger,int,int,string,System.Exception?)"/>.
    /// </remarks>
    /// <seealso cref="string.Format(System.IFormatProvider,string,object)"/>
    public static void Fatal( this ICompilerMessageManger self, Position position, string format, params object[] parameters )
        => Append( self, CompilerMessageLevel.Fatal, position, string.Format( format, parameters ) );

    #endregion ~Alias with specific position / Format Support
}
