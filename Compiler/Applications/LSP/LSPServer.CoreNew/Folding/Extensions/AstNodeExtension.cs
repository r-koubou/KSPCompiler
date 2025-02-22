using KspPosition = KSPCompiler.Commons.Text.Position;

namespace KSPCompiler.Applications.LSPServer.CoreNew.Folding.Extensions;

public static class AstNodeExtension
{
    public static FoldingItem AsFoldingRange( this KspPosition self )
        => new()
        {
            Position = self,
            Kind     = FoldingRangeKind.Region
        };
}
