using System.Threading;
using System.Threading.Tasks;

namespace KSPCompiler.Commons.Contents;

public interface IContentReader
{
    byte[] ReadContent()
        => ReadContentAsync().GetAwaiter().GetResult();

    Task<byte[]> ReadContentAsync( CancellationToken cancellationToken = default );
}
