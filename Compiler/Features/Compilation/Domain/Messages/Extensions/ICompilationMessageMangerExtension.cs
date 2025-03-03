using System;

using KSPCompiler.Shared.Text;

namespace KSPCompiler.Features.Compilation.Domain.Messages.Extensions;

/// <summary>
/// Extension for <see cref="ICompilationMessageManger"/>
/// </summary>
// ReSharper disable once InconsistentNaming
public static class ICompilationMessageMangerExtension
{
    #region Alias with specific line and column

    /// <summary>
    /// Alias for <see cref="ICompilationMessageManger.Append(CompilationMessage)"/>.
    /// </summary>
    private static void Append( this ICompilationMessageManger self, CompilationMessageLevel level, int lineNo, int column, string message, Exception? exception = null )
        => self.Append( self.MessageFactory.Create( level, lineNo, column, message, exception ) );

    /// <summary>
    /// Alias for <see cref="ICompilationMessageManger.Append(CompilationMessage)"/> with <see cref="CompilationMessageLevel.Info"/>.
    /// </summary>
    public static void Info( this ICompilationMessageManger self, int lineNo, int column, string message, Exception? exception = null )
        => Append( self, CompilationMessageLevel.Info, lineNo, column, message, exception );

    /// <summary>
    /// Alias for <see cref="ICompilationMessageManger.Append(CompilationMessage)"/> with <see cref="CompilationMessageLevel.Warning"/>.
    /// </summary>
    public static void Warning( this ICompilationMessageManger self, int lineNo, int column, string message, Exception? exception = null )
        => Append( self, CompilationMessageLevel.Warning, lineNo, column, message, exception );

    /// <summary>
    /// Alias for <see cref="ICompilationMessageManger.Append(CompilationMessage)"/> with <see cref="CompilationMessageLevel.Error"/>.
    /// </summary>
    public static void Error( this ICompilationMessageManger self, int lineNo, int column, string message, Exception? exception = null )
        => Append( self, CompilationMessageLevel.Error, lineNo, column, message, exception );

    /// <summary>
    /// Alias for <see cref="ICompilationMessageManger.Append(CompilationMessage)"/> with <see cref="CompilationMessageLevel.Fatal"/>.
    /// </summary>
    public static void Fatal( this ICompilationMessageManger self, int lineNo, int column, string message, Exception? exception = null )
        => Append( self, CompilationMessageLevel.Fatal, lineNo, column, message, exception );

    #endregion ~Alias with specific line and column

    #region Alias with specific position

    /// <summary>
    /// Alias for <see cref="ICompilationMessageManger.Append(CompilationMessage)"/>.
    /// </summary>
    private static void Append( this ICompilationMessageManger self, CompilationMessageLevel level, Position position, string message, Exception? exception = null )
        => self.Append( self.MessageFactory.Create( level, position, message, exception ) );

    /// <summary>
    /// Alias for <see cref="ICompilationMessageManger.Append(CompilationMessage)"/> with <see cref="CompilationMessageLevel.Info"/>.
    /// </summary>
    public static void Info( this ICompilationMessageManger self, Position position, string message, Exception? exception = null )
        => Append( self, CompilationMessageLevel.Info, position, message, exception );

    /// <summary>
    /// Alias for <see cref="ICompilationMessageManger.Append(CompilationMessage)"/> with <see cref="CompilationMessageLevel.Warning"/>.
    /// </summary>
    public static void Warning( this ICompilationMessageManger self, Position position, string message, Exception? exception = null )
        => Append( self, CompilationMessageLevel.Warning, position, message, exception );

    /// <summary>
    /// Alias for <see cref="ICompilationMessageManger.Append(CompilationMessage)"/> with <see cref="CompilationMessageLevel.Error"/>.
    /// </summary>
    public static void Error( this ICompilationMessageManger self, Position position, string message, Exception? exception = null )
        => Append( self, CompilationMessageLevel.Error, position, message, exception );

    /// <summary>
    /// Alias for <see cref="ICompilationMessageManger.Append(CompilationMessage)"/> with <see cref="CompilationMessageLevel.Fatal"/>.
    /// </summary>
    public static void Fatal( this ICompilationMessageManger self, Position position, string message, Exception? exception = null )
        => Append( self, CompilationMessageLevel.Fatal, position, message, exception );

    #endregion ~Alias with specific position

    #region Alias with specific line and column / Format Support

    /// <summary>
    /// Alias for <see cref="ICompilationMessageManger.Append(CompilationMessage)"/> with <see cref="CompilationMessageLevel.Info"/>.
    /// </summary>
    /// <remarks>
    /// Format support version of <see cref="Info(ICompilationMessageManger,int,int,string,System.Exception?)"/>.
    /// </remarks>
    /// <seealso cref="string.Format(System.IFormatProvider,string,object)"/>
    public static void Info( this ICompilationMessageManger self, int lineNo, int column, string format, params object[] parameters )
        => Append( self, CompilationMessageLevel.Info, lineNo, column, string.Format( format, parameters ) );

    /// <summary>
    /// Alias for <see cref="ICompilationMessageManger.Append(CompilationMessage)"/> with <see cref="CompilationMessageLevel.Warning"/>.
    /// </summary>
    /// <remarks>
    /// Format support version of <see cref="Warning(ICompilationMessageManger,int,int,string,System.Exception?)"/>.
    /// </remarks>
    /// <seealso cref="string.Format(System.IFormatProvider,string,object)"/>
    public static void Warning( this ICompilationMessageManger self, int lineNo, int column, string format, params object[] parameters )
        => Append( self, CompilationMessageLevel.Warning, lineNo, column, string.Format( format, parameters ) );

    /// <summary>
    /// Alias for <see cref="ICompilationMessageManger.Append(CompilationMessage)"/> with <see cref="CompilationMessageLevel.Error"/>.
    /// </summary>
    /// <remarks>
    /// Format support version of <see cref="Error(ICompilationMessageManger,int,int,string,System.Exception?)"/>.
    /// </remarks>
    /// <seealso cref="string.Format(System.IFormatProvider,string,object)"/>
    public static void Error( this ICompilationMessageManger self, int lineNo, int column, string format, params object[] parameters )
        => Append( self, CompilationMessageLevel.Error, lineNo, column, string.Format( format, parameters ) );

    /// <summary>
    /// Alias for <see cref="ICompilationMessageManger.Append(CompilationMessage)"/> with <see cref="CompilationMessageLevel.Fatal"/>.
    /// </summary>
    /// <remarks>
    /// Format support version of <see cref="Fatal(ICompilationMessageManger,int,int,string,System.Exception?)"/>.
    /// </remarks>
    /// <seealso cref="string.Format(System.IFormatProvider,string,object)"/>
    public static void Fatal( this ICompilationMessageManger self, int lineNo, int column, string format, params object[] parameters )
        => Append( self, CompilationMessageLevel.Fatal, lineNo, column, string.Format( format, parameters ) );

    #endregion ~Alias with specific line and column / Format Support

    #region Alias with specific position / Format Support

    /// <summary>
    /// Alias for <see cref="ICompilationMessageManger.Append(CompilationMessage)"/> with <see cref="CompilationMessageLevel.Info"/>.
    /// </summary>
    /// <remarks>
    /// Format support version of <see cref="Info(ICompilationMessageManger,int,int,string,System.Exception?)"/>.
    /// </remarks>
    /// <seealso cref="string.Format(System.IFormatProvider,string,object)"/>
    public static void Info( this ICompilationMessageManger self, Position position, string format, params object[] parameters )
        => Append( self, CompilationMessageLevel.Info, position, string.Format( format, parameters ) );

    /// <summary>
    /// Alias for <see cref="ICompilationMessageManger.Append(CompilationMessage)"/> with <see cref="CompilationMessageLevel.Warning"/>.
    /// </summary>
    /// <remarks>
    /// Format support version of <see cref="Warning(ICompilationMessageManger,int,int,string,System.Exception?)"/>.
    /// </remarks>
    /// <seealso cref="string.Format(System.IFormatProvider,string,object)"/>
    public static void Warning( this ICompilationMessageManger self, Position position, string format, params object[] parameters )
        => Append( self, CompilationMessageLevel.Warning, position, string.Format( format, parameters ) );

    /// <summary>
    /// Alias for <see cref="ICompilationMessageManger.Append(CompilationMessage)"/> with <see cref="CompilationMessageLevel.Error"/>.
    /// </summary>
    /// <remarks>
    /// Format support version of <see cref="Error(ICompilationMessageManger,int,int,string,System.Exception?)"/>.
    /// </remarks>
    /// <seealso cref="string.Format(System.IFormatProvider,string,object)"/>
    public static void Error( this ICompilationMessageManger self, Position position, string format, params object[] parameters )
        => Append( self, CompilationMessageLevel.Error, position, string.Format( format, parameters ) );

    /// <summary>
    /// Alias for <see cref="ICompilationMessageManger.Append(CompilationMessage)"/> with <see cref="CompilationMessageLevel.Fatal"/>.
    /// </summary>
    /// <remarks>
    /// Format support version of <see cref="Fatal(ICompilationMessageManger,int,int,string,System.Exception?)"/>.
    /// </remarks>
    /// <seealso cref="string.Format(System.IFormatProvider,string,object)"/>
    public static void Fatal( this ICompilationMessageManger self, Position position, string format, params object[] parameters )
        => Append( self, CompilationMessageLevel.Fatal, position, string.Format( format, parameters ) );

    #endregion ~Alias with specific position / Format Support
}
