using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Symbols.Extensions;

public static class AstVariableDeclarationExtension
{
    public static VariableSymbol As( this AstVariableDeclarationNode self )
    {
        var result = new VariableSymbol
        {
            Name             = self.Name,
            Reserved         = false,
            Description      = $"Created from {nameof( AstVariableDeclarationExtension )}.{nameof( As )}",
            DataType         = DataTypeUtility.GuessFromSymbolName( new SymbolName( self.Name ) ),
            DataTypeModifier = DataTypeModifierFlag.None
        };

        if( !string.IsNullOrEmpty( self.Modifier ) )
        {
            result.DataTypeModifier |= ( self.Modifier ) switch
            {
                "const"      => DataTypeModifierFlag.Const,
                "polyphonic" => DataTypeModifierFlag.Polyphonic,
                // キーワード以外の文字列が含まれる場合は暫定で UI として、以降で判定
                _ => DataTypeModifierFlag.UI
            };
        }

        return result;
    }
}
