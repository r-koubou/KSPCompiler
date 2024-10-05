using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.CompilerMessages;

namespace KSPCompiler.Domain.Ast.Extensions;

/// <summary>
/// Extension for <see cref="ICompilerMessageManger"/>
/// </summary>
// ReSharper disable once InconsistentNaming
public static class ICompilerMessageMangerExtension
{
    #region Alias with specific position
    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/>.
    /// </summary>
    private static void Append( this ICompilerMessageManger self, CompilerMessageLevel level, IAstNode node, string message, Exception? exception = null )
        => self.Append( self.MessageFactory.Create( level, node.Position, message, exception ) );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/> with <see cref="CompilerMessageLevel.Info"/>.
    /// </summary>
    public static void Info( this ICompilerMessageManger self, IAstNode node, string format, Exception? exception = null )
        => Append( self, CompilerMessageLevel.Info, node, format, exception );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/> with <see cref="CompilerMessageLevel.Info"/>.
    /// </summary>
    public static void Warning( this ICompilerMessageManger self, IAstNode node, string message, Exception? exception = null )
        => Append( self, CompilerMessageLevel.Warning, node, message, exception );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/> with <see cref="CompilerMessageLevel.Info"/>.
    /// </summary>
    public static void Error( this ICompilerMessageManger self, IAstNode node, string message, Exception? exception = null )
        => Append( self, CompilerMessageLevel.Error, node, message, exception );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/> with <see cref="CompilerMessageLevel.Info"/>.
    /// </summary>
    public static void Fatal( this ICompilerMessageManger self, IAstNode node, string message, Exception? exception = null )
        => Append( self, CompilerMessageLevel.Fatal, node, message, exception );

    #endregion

    #region Alias with specific position / Format Support

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/> with <see cref="CompilerMessageLevel.Info"/>.
    /// </summary>
    public static void Info( this ICompilerMessageManger self, IAstNode node, string format, params object[] parameters )
        => Append( self, CompilerMessageLevel.Info, node, string.Format( format, parameters ) );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/> with <see cref="CompilerMessageLevel.Warning"/>.
    /// </summary>
    public static void Warning( this ICompilerMessageManger self, IAstNode node, string format, params object[] parameters )
        => Append( self, CompilerMessageLevel.Warning, node, string.Format( format, parameters ) );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/> with <see cref="CompilerMessageLevel.Error"/>.
    /// </summary>
    public static void Error( this ICompilerMessageManger self, IAstNode node, string format, params object[] parameters )
        => Append( self, CompilerMessageLevel.Error, node, string.Format( format, parameters ) );

    /// <summary>
    /// Alias for <see cref="ICompilerMessageManger.Append(CompilerMessage)"/> with <see cref="CompilerMessageLevel.Fatal"/>.
    /// </summary>
    public static void Fatal( this ICompilerMessageManger self, IAstNode node, string format, params object[] parameters )
        => Append( self, CompilerMessageLevel.Fatal, node, string.Format( format, parameters ) );

    #endregion
}
