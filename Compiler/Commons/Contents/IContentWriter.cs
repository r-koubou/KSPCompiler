using System;
using System.Threading;
using System.Threading.Tasks;

namespace KSPCompiler.Commons.Contents;

public interface IContentWriter
{
    void WriteContent( byte[] content, int start, int length )
        => WriteContentAsync( content, start, length ).GetAwaiter().GetResult();

    async Task WriteContentAsync( byte[] content, int start, int length, CancellationToken cancellationToken = default )
        => await WriteContentAsync( new Span<byte>( content, start, length ), cancellationToken );

    void WriteContent( byte[] content )
        => WriteContentAsync( new Span<byte>( content ) ).GetAwaiter().GetResult();

    async Task WriteContentAsync( byte[] content, CancellationToken cancellationToken = default )
        => await WriteContentAsync( new Span<byte>( content ), cancellationToken );

    void WriteContent( Span<byte> content )
        => WriteContentAsync( content ).GetAwaiter().GetResult();

    Task WriteContentAsync( Span<byte> content, CancellationToken cancellationToken = default );
}
