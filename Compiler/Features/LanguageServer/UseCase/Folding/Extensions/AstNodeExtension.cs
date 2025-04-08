using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Folding;
using KSPCompiler.Shared.Text;

namespace KSPCompiler.Features.LanguageServer.UseCase.Folding.Extensions;

public static class AstNodeExtension
{
    public static FoldingItem AsFoldingRange( this Position self )
        => new()
        {
            Position = self,
            Kind     = FoldingRangeKind.Region
        };
}
