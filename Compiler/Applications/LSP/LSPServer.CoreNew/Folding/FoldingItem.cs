using KSPCompiler.Commons.Text;

namespace KSPCompiler.Applications.LSPServer.CoreNew.Folding;

public sealed record FoldingItem
{
    public Position Position { get; init; }
    public FoldingRangeKind? Kind { get; init; }
}
