using System.IO;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons.Contents;
using KSPCompiler.Commons.Path;

namespace KSPCompiler.Infrastructures.Commons.LocalStorages;

public sealed class LocalBinaryContentWriter : IBinaryContentWriter
{
    // ReSharper disable MemberCanBePrivate.Global
    public FilePath FilePath { get; }
    // ReSharper restore MemberCanBePrivate.Global

    public LocalBinaryContentWriter( FilePath filePath )
    {
        FilePath = filePath;
    }

    public async Task WriteContentAsync( byte[] content, CancellationToken cancellationToken = default )
        => await File.WriteAllBytesAsync( FilePath.Path, content, cancellationToken );
}
