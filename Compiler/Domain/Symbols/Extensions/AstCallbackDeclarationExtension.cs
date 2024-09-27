using KSPCompiler.Domain.Ast.Node.Blocks;
using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Symbols.Extensions;

public static class AstCallbackDeclarationExtension
{
    public static CallbackSymbol As( this AstCallbackDeclaration self, bool requiredDeclareOnInit = false )
    {
        var result = new CallbackSymbol( false )
        {
            Name = self.Name,
            Reserved = false,
            Description = $"Created from {nameof(AstCallbackDeclarationExtension)}.{nameof(As)}",
            DataType = DataTypeFlag.None
        };

        foreach ( var arg in self.ArgumentList.Arguments )
        {
            var argument = new CallbackArgumentSymbol( false )
            {
                Name = arg.Name,
                Reserved = false
            };

            argument.DataType = DataTypeUtility.Guess( argument.Name );
            result.AddArgument( argument );
        }

        return result;
    }
}
