using KSPCompiler.Shared.Text;

namespace KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Symbol;

public record DocumentSymbol
{
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    public string Name { get; init; } = string.Empty;
    public string Detail { get; init; } = string.Empty;
    public SymbolKind Kind { get; init; }
    public Position Range { get; init; }
    public Position SelectionRange { get; init; }
    // ReSharper restore UnusedAutoPropertyAccessor.Global
}
