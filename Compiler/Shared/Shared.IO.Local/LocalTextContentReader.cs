using System.IO;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Shared.IO.Abstractions.Contents;
using KSPCompiler.Shared.Path;

namespace KSPCompiler.Shared.IO.Local;

public sealed class LocalTextContentReader : ITextContentReader
{
    // ReSharper disable MemberCanBePrivate.Global
    public FilePath FilePath { get; }
    // ReSharper restore MemberCanBePrivate.Global

    public LocalTextContentReader( FilePath filePath )
    {
        FilePath = filePath;
    }

    public async Task<string> ReadContentAsync( CancellationToken cancellationToken = default )
        => await File.ReadAllTextAsync( FilePath.Path, cancellationToken );
}
