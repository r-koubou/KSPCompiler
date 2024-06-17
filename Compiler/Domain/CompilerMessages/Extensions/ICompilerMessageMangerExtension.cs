namespace KSPCompiler.Domain.CompilerMessages.Extensions;

/// <summary>
/// Extension for <see cref="ICompilerMessageManger"/>
/// </summary>
// ReSharper disable once InconsistentNaming
public static class ICompilerMessageMangerExtension
{
    #region Alias

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/>.
    /// </summary>
    public static void Append( this ICompilerMessageManger self, CompilerMessageLevel level, string message )
        => self.Append( self.MessageFactory.Create( level, message ) );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/> with <see cref="CompilerMessageLevel.Info"/>.
    /// </summary>
    public static void Info( this ICompilerMessageManger self, string message )
        => self.Append( self.MessageFactory.Info( message ) );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/> with <see cref="CompilerMessageLevel.Warning"/>.
    /// </summary>
    public static void Warning( this ICompilerMessageManger self, string message )
        => self.Append( self.MessageFactory.Warning( message ) );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/> with <see cref="CompilerMessageLevel.Error"/>.
    /// </summary>
    public static void Error( this ICompilerMessageManger self, string message )
        => self.Append( self.MessageFactory.Error( message ) );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/> with <see cref="CompilerMessageLevel.Fatal"/>.
    /// </summary>
    public static void Fatal( this ICompilerMessageManger self, string message )
        => self.Append( self.MessageFactory.Fatal( message ) );

    #endregion ~Alias

    #region Alias and Format Support

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/>.
    /// </summary>
    /// <remarks>
    /// Format support version of <see cref="Append(KSPCompiler.Domain.CompilerMessages.ICompilerMessageManger,KSPCompiler.Domain.CompilerMessages.CompilerMessageLevel,string)" />.
    /// </remarks>
    /// <seealso cref="string.Format(System.IFormatProvider,string,object)"/>
    public static void Append( this ICompilerMessageManger self, CompilerMessageLevel level, string format, params object[] parameters )
        => self.Append( self.MessageFactory.Create( level, string.Format( format, parameters ) ) );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/> with <see cref="CompilerMessageLevel.Info"/>.
    /// </summary>
    /// <remarks>
    /// Format support version of <see cref="Info(ICompilerMessageManger, string)"/>.
    /// </remarks>
    /// <seealso cref="string.Format(System.IFormatProvider,string,object)"/>
    public static void Info( this ICompilerMessageManger self, string format, params object[] parameters )
        => self.Append( self.MessageFactory.Info( string.Format( format, parameters ) ) );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/> with <see cref="CompilerMessageLevel.Warning"/>.
    /// </summary>
    /// <remarks>
    /// Format support version of <see cref="Warning(ICompilerMessageManger, string)"/>.
    /// </remarks>
    /// <seealso cref="string.Format(System.IFormatProvider,string,object)"/>
    public static void Warning( this ICompilerMessageManger self, string format, params object[] parameters )
        => self.Append( self.MessageFactory.Warning( string.Format( format, parameters ) ) );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/> with <see cref="CompilerMessageLevel.Error"/>.
    /// </summary>
    /// <remarks>
    /// Format support version of <see cref="Error(ICompilerMessageManger, string)"/>.
    /// </remarks>
    /// <seealso cref="string.Format(System.IFormatProvider,string,object)"/>
    public static void Error( this ICompilerMessageManger self, string format, params object[] parameters )
        => self.Append( self.MessageFactory.Error( string.Format( format, parameters ) ) );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/> with <see cref="CompilerMessageLevel.Fatal"/>.
    /// </summary>
    /// <remarks>
    /// Format support version of <see cref="Fatal(ICompilerMessageManger, string)"/>.
    /// </remarks>
    /// <seealso cref="string.Format(System.IFormatProvider,string,object)"/>
    public static void Fatal( this ICompilerMessageManger self, string format, params object[] parameters )
        => self.Append( self.MessageFactory.Fatal( string.Format( format, parameters ) ) );

    #endregion ~Alias and Format Support
}
