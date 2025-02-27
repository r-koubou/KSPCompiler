using KSPCompiler.UseCases.LanguageServer.Folding;

using KspPosition = KSPCompiler.Commons.Text.Position;

namespace KSPCompiler.Interactors.LanguageServer.Folding.Extensions;

public static class AstNodeExtension
{
    public static FoldingItem AsFoldingRange( this KspPosition self )
        => new()
        {
            Position = self,
            Kind     = FoldingRangeKind.Region
        };
}
