using System;

using KSPCompiler.Shared.Text;

namespace KSPCompiler.Features.Compilation.Domain.Messages.Extensions;

/// <summary>
/// Extension for <see cref="ICompilationMessageFactory"/>
/// </summary>
// ReSharper disable once InconsistentNaming
public static class ICompilationMessageFactoryExtension
{
    #region Alias with specific line and column
    /// <summary>
    /// Alias for <see cref="ICompilationMessageFactory.Create(CompilationMessageLevel,Position,string,System.Exception?)"/>.
    /// </summary>
    private static CompilationMessage Create( this ICompilationMessageFactory self, CompilationMessageLevel level, int lineNo, int column, string message, Exception? exception = null )
        => self.Create( level, lineNo, column, message, exception );

    /// <summary>
    /// Alias for <see cref="ICompilationMessageFactory.Create(CompilationMessageLevel,Position,string,System.Exception?)"/> with <see cref="CompilationMessageLevel.Info"/>.
    /// </summary>
    public static CompilationMessage Info( this ICompilationMessageFactory self, int lineNo, int column, string message, Exception? exception = null )
        => Create( self, CompilationMessageLevel.Info, lineNo, column, message, exception );

    /// <summary>
    /// Alias for <see cref="ICompilationMessageFactory.Create(CompilationMessageLevel,Position,string,System.Exception?)"/> with <see cref="CompilationMessageLevel.Warning"/>.
    /// </summary>
    public static CompilationMessage Warning( this ICompilationMessageFactory self, int lineNo, int column, string message, Exception? exception = null )
        => Create( self, CompilationMessageLevel.Warning, lineNo, column, message, exception );

    /// <summary>
    /// Alias for <see cref="ICompilationMessageFactory.Create(CompilationMessageLevel,Position,string,System.Exception?)"/> with <see cref="CompilationMessageLevel.Error"/>.
    /// </summary>
    public static CompilationMessage Error( this ICompilationMessageFactory self, int lineNo, int column, string message, Exception? exception = null )
        => Create( self, CompilationMessageLevel.Error, lineNo, column, message, exception );

    /// <summary>
    /// Alias for <see cref="ICompilationMessageFactory.Create(CompilationMessageLevel,Position,string,System.Exception?)"/> with <see cref="CompilationMessageLevel.Fatal"/>.
    /// </summary>
    public static CompilationMessage Fatal( this ICompilationMessageFactory self, int lineNo, int column, string message, Exception? exception = null )
        => Create( self, CompilationMessageLevel.Fatal, lineNo, column, message, exception );

    #endregion ~Alias with specific line and column

    #region Alias with specific posision

    /// <summary>
    /// Alias for <see cref="ICompilationMessageFactory.Create(CompilationMessageLevel,Position,string,System.Exception?)"/>.
    /// </summary>
    private static CompilationMessage Create( this ICompilationMessageFactory self, CompilationMessageLevel level, Position position, string message, Exception? exception = null )
        => self.Create( level, position, message, exception );

    /// <summary>
    /// Alias for <see cref="ICompilationMessageFactory.Create(CompilationMessageLevel,Position,string,System.Exception?)"/> with <see cref="CompilationMessageLevel.Info"/>.
    /// </summary>
    public static CompilationMessage Info( this ICompilationMessageFactory self, Position position, string message, Exception? exception = null )
        => Create( self, CompilationMessageLevel.Info, position, message, exception );

    /// <summary>
    /// Alias for <see cref="ICompilationMessageFactory.Create(CompilationMessageLevel,Position,string,System.Exception?)"/> with <see cref="CompilationMessageLevel.Warning"/>.
    /// </summary>
    public static CompilationMessage Warning( this ICompilationMessageFactory self, Position position, string message, Exception? exception = null )
        => Create( self, CompilationMessageLevel.Warning, position, message, exception );

    /// <summary>
    /// Alias for <see cref="ICompilationMessageFactory.Create(CompilationMessageLevel,Position,string,System.Exception?)"/> with <see cref="CompilationMessageLevel.Error"/>.
    /// </summary>
    public static CompilationMessage Error( this ICompilationMessageFactory self, Position position, string message, Exception? exception = null )
        => Create( self, CompilationMessageLevel.Error, position, message, exception );

    /// <summary>
    /// Alias for <see cref="ICompilationMessageFactory.Create(CompilationMessageLevel,Position,string,System.Exception?)"/> with <see cref="CompilationMessageLevel.Fatal"/>.
    /// </summary>
    public static CompilationMessage Fatal( this ICompilationMessageFactory self, Position position, string message, Exception? exception = null )
        => Create( self, CompilationMessageLevel.Fatal, position, message, exception );

    #endregion


    #region Alias with specific line and column / Format Support

    /// <summary>
    /// Alias for <see cref="ICompilationMessageFactory.Create(CompilationMessageLevel,Position,string,System.Exception?)"/> with <see cref="CompilationMessageLevel.Info"/>.
    /// </summary>
    /// <remarks>
    /// Format support version of <see cref="Info(ICompilationMessageFactory,int,int,string,System.Exception?)"/>
    /// </remarks>
    /// <seealso cref="string.Format(System.IFormatProvider,string,object)"/>
    public static CompilationMessage Info( this ICompilationMessageFactory self, int lineNo, int column, string message, params object[] parameters )
        => Create( self, CompilationMessageLevel.Info, lineNo, column, string.Format( message, parameters ) );

    /// <summary>
    /// Alias for <see cref="ICompilationMessageFactory.Create(CompilationMessageLevel,Position,string,System.Exception?)"/> with <see cref="CompilationMessageLevel.Warning"/>.
    /// </summary>
    /// <remarks>
    /// Format support version of <see cref="Warning(ICompilationMessageFactory,int,int,string,System.Exception?)"/>
    /// </remarks>
    /// <seealso cref="string.Format(System.IFormatProvider,string,object)"/>
    public static CompilationMessage Warning( this ICompilationMessageFactory self, int lineNo, int column, string message, params object[] parameters )
        => Create( self, CompilationMessageLevel.Warning, lineNo, column, string.Format( message, parameters ) );

    /// <summary>
    /// Alias for <see cref="ICompilationMessageFactory.Create(CompilationMessageLevel,Position,string,System.Exception?)"/> with <see cref="CompilationMessageLevel.Error"/>.
    /// </summary>
    /// <remarks>
    /// Format support version of <see cref="Error(ICompilationMessageFactory,int,int,string,System.Exception?)"/>
    /// </remarks>
    /// <seealso cref="string.Format(System.IFormatProvider,string,object)"/>
    public static CompilationMessage Error( this ICompilationMessageFactory self, int lineNo, int column, string message, params object[] parameters )
        => Create( self, CompilationMessageLevel.Error, lineNo, column, string.Format( message, parameters ) );

    /// <summary>
    /// Alias for <see cref="ICompilationMessageFactory.Create(CompilationMessageLevel,Position,string,System.Exception?)"/> with <see cref="CompilationMessageLevel.Fatal"/>.
    /// </summary>
    /// <remarks>
    /// Format support version of <see cref="Fatal(ICompilationMessageFactory,int,int,string,System.Exception?)"/>
    /// </remarks>
    /// <seealso cref="string.Format(System.IFormatProvider,string,object)"/>
    public static CompilationMessage Fatal( this ICompilationMessageFactory self, int lineNo, int column, string message, params object[] parameters )
        => Create( self, CompilationMessageLevel.Fatal, lineNo, column, string.Format( message, parameters ) );

    #endregion

}
