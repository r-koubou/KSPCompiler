using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Statements;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;

namespace KSPCompiler.Shared.Domain.Compilation.Symbols.Extensions;

public static class AstVariableDeclarationExtension
{
    public static VariableSymbol As( this AstVariableDeclarationNode self )
    {
        var result = new VariableSymbol
        {
            Name         = self.Name,
            BuiltIn      = false,
            DataType     = DataTypeUtility.GuessFromSymbolName( new SymbolName( self.Name ) ),
            Modifier     = ModifierFlag.None,
            CommentLines = self.CommentLines
        };

        foreach( var x in self.Modifier.Values )
        {
            result.Modifier |= x switch
            {
                "const"      => ModifierFlag.Const,
                "polyphonic" => ModifierFlag.Polyphonic,
                // キーワード以外の文字列が含まれる場合は暫定で UI として、以降で判定
                _ => ModifierFlag.UI
            };
        }

        return result;
    }
}
