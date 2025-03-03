using KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Features.Compilation.Domain.Symbols.MetaData;

namespace KSPCompiler.Features.Compilation.Domain.Symbols.Extensions;

public static class AstCallbackDeclarationExtension
{
    public static CallbackSymbol As( this AstCallbackDeclarationNode self, bool requiredDeclareOnInit = false )
    {
        var result = new CallbackSymbol( false )
        {
            Name         = self.Name,
            BuiltIn      = false,
            Description  = $"Created from {nameof( AstCallbackDeclarationExtension )}.{nameof( As )}",
            CommentLines = self.CommentLines
        };

        foreach ( var arg in self.ArgumentList.Arguments )
        {
            var argument = new CallbackArgumentSymbol( false )
            {
                Name = arg.Name,
                BuiltIn = false
            };

            argument.DataType = DataTypeUtility.GuessFromSymbolName( argument.Name );
            result.Arguments.Add( argument );
        }

        return result;
    }
}
