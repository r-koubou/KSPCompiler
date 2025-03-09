using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.Compilation.Gateways.Symbols;

public interface IBuiltInSymbolLoader
{
    Task<AggregateSymbolTable> LoadAsync( CancellationToken cancellationToken = default );
}
