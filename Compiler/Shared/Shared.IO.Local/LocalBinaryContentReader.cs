using System.IO;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Shared.IO.Abstractions.Contents;
using KSPCompiler.Shared.Path;

namespace KSPCompiler.Shared.IO.Abstractions.LocalStorages;

public sealed class LocalBinaryContentReader : IBinaryContentReader
{
    // ReSharper disable MemberCanBePrivate.Global
    public FilePath FilePath { get; }
    // ReSharper restore MemberCanBePrivate.Global

    public LocalBinaryContentReader( FilePath filePath )
    {
        FilePath = filePath;
    }

    public async Task<byte[]> ReadContentAsync( CancellationToken cancellationToken = default )
        => await File.ReadAllBytesAsync( FilePath.Path, cancellationToken );
}
