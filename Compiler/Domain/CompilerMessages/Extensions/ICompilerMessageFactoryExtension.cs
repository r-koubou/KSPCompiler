using System;

using KSPCompiler.Commons.Text;

namespace KSPCompiler.Domain.CompilerMessages.Extensions;

/// <summary>
/// Extension for <see cref="ICompilerMessageFactory"/>
/// </summary>
// ReSharper disable once InconsistentNaming
public static class ICompilerMessageFactoryExtension
{
    #region Alias with specific line and column
    /// <summary>
    /// Alias for <see cref="ICompilerMessageFactory.Create(KSPCompiler.Domain.CompilerMessages.CompilerMessageLevel,KSPCompiler.Commons.Text.Position,string,System.Exception?)"/>.
    /// </summary>
    private static CompilerMessage Create( this ICompilerMessageFactory self, CompilerMessageLevel level, int lineNo, int column, string message, Exception? exception = null )
        => self.Create( level, lineNo, column, message, exception );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageFactory.Create(KSPCompiler.Domain.CompilerMessages.CompilerMessageLevel,KSPCompiler.Commons.Text.Position,string,System.Exception?)"/> with <see cref="CompilerMessageLevel.Info"/>.
    /// </summary>
    public static CompilerMessage Info( this ICompilerMessageFactory self, int lineNo, int column, string message, Exception? exception = null )
        => Create( self, CompilerMessageLevel.Info, lineNo, column, message, exception );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageFactory.Create(KSPCompiler.Domain.CompilerMessages.CompilerMessageLevel,KSPCompiler.Commons.Text.Position,string,System.Exception?)"/> with <see cref="CompilerMessageLevel.Warning"/>.
    /// </summary>
    public static CompilerMessage Warning( this ICompilerMessageFactory self, int lineNo, int column, string message, Exception? exception = null )
        => Create( self, CompilerMessageLevel.Warning, lineNo, column, message, exception );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageFactory.Create(KSPCompiler.Domain.CompilerMessages.CompilerMessageLevel,KSPCompiler.Commons.Text.Position,string,System.Exception?)"/> with <see cref="CompilerMessageLevel.Error"/>.
    /// </summary>
    public static CompilerMessage Error( this ICompilerMessageFactory self, int lineNo, int column, string message, Exception? exception = null )
        => Create( self, CompilerMessageLevel.Error, lineNo, column, message, exception );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageFactory.Create(KSPCompiler.Domain.CompilerMessages.CompilerMessageLevel,KSPCompiler.Commons.Text.Position,string,System.Exception?)"/> with <see cref="CompilerMessageLevel.Fatal"/>.
    /// </summary>
    public static CompilerMessage Fatal( this ICompilerMessageFactory self, int lineNo, int column, string message, Exception? exception = null )
        => Create( self, CompilerMessageLevel.Fatal, lineNo, column, message, exception );

    #endregion ~Alias with specific line and column

    #region Alias with specific posision

    /// <summary>
    /// Alias for <see cref="ICompilerMessageFactory.Create(KSPCompiler.Domain.CompilerMessages.CompilerMessageLevel,KSPCompiler.Commons.Text.Position,string,System.Exception?)"/>.
    /// </summary>
    private static CompilerMessage Create( this ICompilerMessageFactory self, CompilerMessageLevel level, Position position, string message, Exception? exception = null )
        => self.Create( level, position, message, exception );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageFactory.Create(KSPCompiler.Domain.CompilerMessages.CompilerMessageLevel,KSPCompiler.Commons.Text.Position,string,System.Exception?)"/> with <see cref="CompilerMessageLevel.Info"/>.
    /// </summary>
    public static CompilerMessage Info( this ICompilerMessageFactory self, Position position, string message, Exception? exception = null )
        => Create( self, CompilerMessageLevel.Info, position, message, exception );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageFactory.Create(KSPCompiler.Domain.CompilerMessages.CompilerMessageLevel,KSPCompiler.Commons.Text.Position,string,System.Exception?)"/> with <see cref="CompilerMessageLevel.Warning"/>.
    /// </summary>
    public static CompilerMessage Warning( this ICompilerMessageFactory self, Position position, string message, Exception? exception = null )
        => Create( self, CompilerMessageLevel.Warning, position, message, exception );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageFactory.Create(KSPCompiler.Domain.CompilerMessages.CompilerMessageLevel,KSPCompiler.Commons.Text.Position,string,System.Exception?)"/> with <see cref="CompilerMessageLevel.Error"/>.
    /// </summary>
    public static CompilerMessage Error( this ICompilerMessageFactory self, Position position, string message, Exception? exception = null )
        => Create( self, CompilerMessageLevel.Error, position, message, exception );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageFactory.Create(KSPCompiler.Domain.CompilerMessages.CompilerMessageLevel,KSPCompiler.Commons.Text.Position,string,System.Exception?)"/> with <see cref="CompilerMessageLevel.Fatal"/>.
    /// </summary>
    public static CompilerMessage Fatal( this ICompilerMessageFactory self, Position position, string message, Exception? exception = null )
        => Create( self, CompilerMessageLevel.Fatal, position, message, exception );

    #endregion


    #region Alias with specific line and column / Format Support

    /// <summary>
    /// Alias for <see cref="ICompilerMessageFactory.Create(KSPCompiler.Domain.CompilerMessages.CompilerMessageLevel,KSPCompiler.Commons.Text.Position,string,System.Exception?)"/> with <see cref="CompilerMessageLevel.Info"/>.
    /// </summary>
    /// <remarks>
    /// Format support version of <see cref="Info(KSPCompiler.Domain.CompilerMessages.ICompilerMessageFactory,int,int,string,System.Exception?)"/>
    /// </remarks>
    /// <seealso cref="string.Format(System.IFormatProvider,string,object)"/>
    public static CompilerMessage Info( this ICompilerMessageFactory self, int lineNo, int column, string message, params object[] parameters )
        => Create( self, CompilerMessageLevel.Info, lineNo, column, string.Format( message, parameters ) );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageFactory.Create(KSPCompiler.Domain.CompilerMessages.CompilerMessageLevel,KSPCompiler.Commons.Text.Position,string,System.Exception?)"/> with <see cref="CompilerMessageLevel.Warning"/>.
    /// </summary>
    /// <remarks>
    /// Format support version of <see cref="Warning(KSPCompiler.Domain.CompilerMessages.ICompilerMessageFactory,int,int,string,System.Exception?)"/>
    /// </remarks>
    /// <seealso cref="string.Format(System.IFormatProvider,string,object)"/>
    public static CompilerMessage Warning( this ICompilerMessageFactory self, int lineNo, int column, string message, params object[] parameters )
        => Create( self, CompilerMessageLevel.Warning, lineNo, column, string.Format( message, parameters ) );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageFactory.Create(KSPCompiler.Domain.CompilerMessages.CompilerMessageLevel,KSPCompiler.Commons.Text.Position,string,System.Exception?)"/> with <see cref="CompilerMessageLevel.Error"/>.
    /// </summary>
    /// <remarks>
    /// Format support version of <see cref="Error(KSPCompiler.Domain.CompilerMessages.ICompilerMessageFactory,int,int,string,System.Exception?)"/>
    /// </remarks>
    /// <seealso cref="string.Format(System.IFormatProvider,string,object)"/>
    public static CompilerMessage Error( this ICompilerMessageFactory self, int lineNo, int column, string message, params object[] parameters )
        => Create( self, CompilerMessageLevel.Error, lineNo, column, string.Format( message, parameters ) );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageFactory.Create(KSPCompiler.Domain.CompilerMessages.CompilerMessageLevel,KSPCompiler.Commons.Text.Position,string,System.Exception?)"/> with <see cref="CompilerMessageLevel.Fatal"/>.
    /// </summary>
    /// <remarks>
    /// Format support version of <see cref="Fatal(KSPCompiler.Domain.CompilerMessages.ICompilerMessageFactory,int,int,string,System.Exception?)"/>
    /// </remarks>
    /// <seealso cref="string.Format(System.IFormatProvider,string,object)"/>
    public static CompilerMessage Fatal( this ICompilerMessageFactory self, int lineNo, int column, string message, params object[] parameters )
        => Create( self, CompilerMessageLevel.Fatal, lineNo, column, string.Format( message, parameters ) );

    #endregion

}
