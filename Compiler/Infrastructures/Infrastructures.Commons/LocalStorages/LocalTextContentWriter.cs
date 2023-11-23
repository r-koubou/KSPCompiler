using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons.Contents;
using KSPCompiler.Commons.Path;

namespace KSPCompiler.Infrastructures.Commons.LocalStorages;

public sealed class LocalTextContentWriter : ITextContentWriter
{
    // ReSharper disable MemberCanBePrivate.Global
    public FilePath FilePath { get; }
    // ReSharper restore MemberCanBePrivate.Global

    public LocalTextContentWriter( FilePath filePath )
    {
        FilePath = filePath;
    }

    public async Task WriteContentAsync( IReadOnlyCollection<string> content, CancellationToken cancellationToken = default )
        => await File.WriteAllLinesAsync( FilePath.Path, content, cancellationToken );
}
