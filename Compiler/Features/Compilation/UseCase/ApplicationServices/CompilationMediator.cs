using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions;

namespace KSPCompiler.Features.Compilation.UseCase.ApplicationServices;

public sealed class CompilationMediator( ICompilationRequestHandler handler ) : ICompilationMediator
{
    private readonly ICompilationRequestHandler handler = handler;

    public async Task<CompilationResponse> RequestAsync( CompilationRequest request, CancellationToken token = default )
        => await handler.HandleAsync( request, token );
}
