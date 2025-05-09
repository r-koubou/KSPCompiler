using System.Threading;
using System.Threading.Tasks;

namespace KSPCompiler.Shared.IO.Abstractions.Contents;

public interface IContentReader<T>
{
    T ReadContent()
        => ReadContentAsync().GetAwaiter().GetResult();

    Task<T> ReadContentAsync( CancellationToken cancellationToken = default );
}
