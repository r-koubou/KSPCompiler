using System.IO;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons.Contents;
using KSPCompiler.Commons.Path;

namespace KSPCompiler.Infrastructures.Commons.LocalStorages;

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
