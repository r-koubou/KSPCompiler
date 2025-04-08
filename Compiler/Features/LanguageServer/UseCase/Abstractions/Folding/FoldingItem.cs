using KSPCompiler.Shared.Text;

namespace KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Folding;

public sealed record FoldingItem
{
    public Position Position { get; init; }
    public FoldingRangeKind Kind { get; init; }
}
