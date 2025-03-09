using System.IO;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Shared.Contents;
using KSPCompiler.Shared.Path;

namespace KSPCompiler.Features.Shared.IO.LocalStorages;

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
