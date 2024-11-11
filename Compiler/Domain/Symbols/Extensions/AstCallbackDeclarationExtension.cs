using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Symbols.Extensions;

public static class AstCallbackDeclarationExtension
{
    public static CallbackSymbol As( this AstCallbackDeclarationNode self, bool requiredDeclareOnInit = false )
    {
        var result = new CallbackSymbol( false )
        {
            Name        = self.Name,
            BuiltIn     = false,
            Description = $"Created from {nameof( AstCallbackDeclarationExtension )}.{nameof( As )}"
        };

        foreach ( var arg in self.ArgumentList.Arguments )
        {
            var argument = new CallbackArgumentSymbol( false )
            {
                Name = arg.Name,
                BuiltIn = false
            };

            argument.DataType = DataTypeUtility.GuessFromSymbolName( argument.Name );
            result.AddArgument( argument );
        }

        return result;
    }
}
