using System.Linq;

using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.LanguageServer.UseCase.Abstractions.SignatureHelp.Extensions;

public static class SymbolTableExtension
{
    private static IOverloadedSignatureHelpBuilder<CommandSymbol> DefaultCommandSignatureHelpBuilder { get; }
        = new CommandSignatureHelpBuilder();

    public static bool TryBuildSignatureHelp(
        this ICommandSymbolTable self,
        string symbolName,
        int activeParameter,
        out SignatureHelpItem result,
        IOverloadedSignatureHelpBuilder<CommandSymbol>? builder = null )
    {
        result = null!;

        if( !self.TrySearchByName( symbolName, out var symbol ) )
        {
            return false;
        }

        var activeSignature = 0;

        // 現在の引数の位置とコマンドで定義しているの引数の数を比較して
        // 引数の数の範囲内に一番近いシグネチャを選択する
        if( activeParameter > 0 )
        {
            var i = 0;
            foreach( var x in symbol )
            {
                if( activeParameter < x.ArgumentCount )
                {
                    activeSignature = i;
                    break;
                }

                i++;
            }
        }

        builder ??= DefaultCommandSignatureHelpBuilder;
        result  =   builder.Build( symbol, activeSignature, activeParameter );

        return result.Signatures.Any();
    }
}
