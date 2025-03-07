using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.SymbolManagement.Gateways;

public interface ISymbolExporter<in TSymbol> where TSymbol : SymbolBase
{
    /// <summary>
    /// Null Object. Symbols export to nowhere.
    /// </summary>
    public static ISymbolExporter<TSymbol> Null { get; } = new NullSymbolExporter<TSymbol>();

    /// <summary>
    /// Export symbol table.
    /// </summary>
    void Export( IEnumerable<TSymbol> symbols )
        => ExportAsync( symbols ).GetAwaiter().GetResult();

    /// <summary>
    /// Export symbol table.
    /// </summary>
    Task ExportAsync( IEnumerable<TSymbol> store, CancellationToken cancellationToken = default );

    /// <summary>
    /// Export symbol table template.
    /// </summary>
    Task ExportTemplateAsync( CancellationToken cancellationToken = default )
        => Task.CompletedTask;
}

/// <summary>
/// Do nothing exporter for Null Object.
/// </summary>
/// <typeparam name="TSymbol"></typeparam>
internal sealed class NullSymbolExporter<TSymbol> : ISymbolExporter<TSymbol> where TSymbol : SymbolBase
{
    public async Task ExportAsync( IEnumerable<TSymbol> store, CancellationToken cancellationToken = default )
        => await Task.CompletedTask;

    public async Task ExportTemplateAsync( CancellationToken cancellationToken = default )
        => await Task.CompletedTask;
}
