using System.IO;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Shared.IO.Abstractions.Contents;
using KSPCompiler.Shared.Path;

namespace KSPCompiler.Shared.IO.Local;

public sealed class LocalTextContentWriter : ITextContentWriter
{
    // ReSharper disable MemberCanBePrivate.Global
    public FilePath FilePath { get; }
    // ReSharper restore MemberCanBePrivate.Global

    public LocalTextContentWriter( FilePath filePath )
    {
        FilePath = filePath;
    }

    public async Task WriteContentAsync( string content, CancellationToken cancellationToken = default )
        => await File.WriteAllTextAsync( FilePath.Path, content, cancellationToken );
}
