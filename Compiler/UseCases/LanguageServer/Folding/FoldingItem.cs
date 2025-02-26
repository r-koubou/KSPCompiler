using KSPCompiler.Commons.Text;

namespace KSPCompiler.UseCases.LanguageServer.Folding;

public sealed record FoldingItem
{
    public Position Position { get; init; }
    public FoldingRangeKind Kind { get; init; }
}
