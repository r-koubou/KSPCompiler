using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks;

namespace KSPCompiler.Shared.Domain.Compilation.Symbols.Extensions;

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
