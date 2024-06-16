namespace KSPCompiler.Domain.CompilerMessages.Extensions;

/// <summary>
/// Extension for <see cref="ICompilerMessageManger"/>
/// </summary>
// ReSharper disable once InconsistentNaming
public static class ICompilerMessageMangerExtension
{
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
}
