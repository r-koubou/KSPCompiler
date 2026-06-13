using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Compilation;
using KSPCompiler.Shared;

namespace KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Symbol;

public sealed record DocumentSymbolInput(
    ICompilationCacheManager Cache,
    ScriptLocation Location
);

public sealed record  DocumentSymbolOutput(
    List<DocumentSymbol> Symbols
);

public interface IDocumentSymbolUseCase
{
    Task<Result<DocumentSymbolOutput, LanguageServerFailureReason>> ExecuteAsync( DocumentSymbolInput input, CancellationToken cancellationToken = default );
}
