using System.Threading;
using System.Threading.Tasks;

namespace KSPCompiler.Shared.IO.Abstractions.Contents;

public interface IContentWriter<in T>
{
    void WriteContent( T content )
        => WriteContentAsync( content ).GetAwaiter().GetResult();

    Task WriteContentAsync( T content, CancellationToken cancellationToken = default );
}
