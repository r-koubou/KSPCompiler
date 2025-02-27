using KspPosition = KSPCompiler.Commons.Text.Position;

namespace KSPCompiler.Applications.LSPServer.Core.Folding.Extensions;

public static class AstNodeExtension
{
    public static FoldingItem AsFoldingRange( this KspPosition self )
        => new()
        {
            Position = self,
            Kind     = FoldingRangeKind.Region
        };
}
