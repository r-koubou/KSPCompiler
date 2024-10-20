using System.IO;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons.Contents;
using KSPCompiler.Commons.Path;

namespace KSPCompiler.Infrastructures.Commons.LocalStorages;

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
