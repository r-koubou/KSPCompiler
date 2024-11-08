using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Symbols.Extensions;

public static class AstUserFunctionDeclarationExtension
{
    public static UserFunctionSymbol As( this AstUserFunctionDeclarationNode self )
    {
        var result = new UserFunctionSymbol
        {
            Name        = self.Name,
            Reserved    = false,
            Description = $"Created from {nameof(AstUserFunctionDeclarationExtension)}.{nameof(As)}"
        };

        return result;
    }
}
