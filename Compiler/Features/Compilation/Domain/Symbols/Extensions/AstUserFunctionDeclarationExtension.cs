using KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Blocks;

namespace KSPCompiler.Features.Compilation.Domain.Symbols.Extensions;

public static class AstUserFunctionDeclarationExtension
{
    public static UserFunctionSymbol As( this AstUserFunctionDeclarationNode self )
    {
        var result = new UserFunctionSymbol
        {
            Name         = self.Name,
            BuiltIn      = false,
            Description  = $"Created from {nameof( AstUserFunctionDeclarationExtension )}.{nameof( As )}",
            CommentLines = self.CommentLines
        };

        return result;
    }
}
