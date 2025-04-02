using System;
using System.Runtime.InteropServices;

namespace KSPCompiler.Features.LanguageServer.UseCase.Abstractions;

public sealed record ScriptLocation( Uri ScriptUri )
{
    public static bool TryParse( string scriptUri, out ScriptLocation scriptLocation )
    {
        scriptLocation = null!;

        if( string.IsNullOrWhiteSpace( scriptUri ) )
        {
            return false;
        }

        if( !Uri.TryCreate( scriptUri, UriKind.RelativeOrAbsolute, out var uri ) )
        {
            return false;
        }

        scriptLocation = new ScriptLocation( uri );

        return true;
    }

    public bool IsUntitled
        => ScriptUri.Scheme.StartsWith( "untitled" );

    public string UnescapedUriString
        => Uri.UnescapeDataString( ScriptUri.AbsoluteUri );

    public string FileSystemPath
    {
        get
        {
            var fileSystemPath = Uri.UnescapeDataString( ScriptUri.AbsolutePath );

            if( !RuntimeInformation.IsOSPlatform( OSPlatform.Windows ) )
            {
                return fileSystemPath;
            }

            if( fileSystemPath.StartsWith( '/' ) )
            {
                fileSystemPath = fileSystemPath.TrimStart( '/' );
            }

            fileSystemPath = fileSystemPath.Replace( '/', '\\' );

            return fileSystemPath;
        }
    }
}
