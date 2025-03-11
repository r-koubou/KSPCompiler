using System.IO;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Shared.IO.Abstractions.Contents;
using KSPCompiler.Shared.Path;

namespace KSPCompiler.Shared.IO.Local;

public sealed class LocalTextContentWriter( FilePath filePath ) : ITextContentWriter
{
    // ReSharper disable MemberCanBePrivate.Global
    public FilePath FilePath { get; } = filePath;
    // ReSharper restore MemberCanBePrivate.Global

    public async Task WriteContentAsync( string content, CancellationToken cancellationToken = default )
    {
        var directory = System.IO.Path.GetDirectoryName( FilePath.Path );

        if( directory != null && !Directory.Exists( directory ) )
        {
            Directory.CreateDirectory( directory );
        }

        await File.WriteAllTextAsync( FilePath.Path, content, cancellationToken );
    }
}
