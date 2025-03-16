using System.Collections.Generic;
using System.Linq;

using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.LanguageServer.UseCase.Abstractions.SignatureHelp;

public class DefaultSignatureHelpBuilder : ISignatureHelpBuilder<CommandSymbol>
{
    public SignatureHelpItem Build( CommandSymbol symbol, int activeParameter )
    {
        return new SignatureHelpItem
        {
            Signatures      = [ BuildInformation( symbol, activeParameter ) ],
            ActiveParameter = activeParameter
        };
    }

    private static SignatureHelpInformationItem BuildInformation( CommandSymbol symbol, int activeParameter )
    {
        var label = string.Empty;

        if( symbol.ArgumentCount > 0 )
        {
            label = $"{symbol.Name.Value}({string.Join( ", ", symbol.Arguments.Select( arg => arg.Name.Value ) )})";
        }

        return new()
        {
            Label = label,
            Documentation = new StringOrMarkdownContent
            {
                IsMarkdown = true,
                Value      = symbol.Description.Value
            },
            Parameters      = BuildParameterInformation( symbol ),
            ActiveParameter = activeParameter
        };
    }

    private static List<SignatureHelpParameterItem> BuildParameterInformation( CommandSymbol symbol )
    {
        if( symbol.ArgumentCount == 0 )
        {
            return [];
        }

        var result = new List<SignatureHelpParameterItem>();

        foreach( var arg in symbol.Arguments )
        {
            var description = !string.IsNullOrEmpty( arg.Description )
                ? arg.Description.Value
                : string.Empty;

            result.Add(
                new SignatureHelpParameterItem
                {
                    Label =　arg.Name.Value,
                    Documentation = new StringOrMarkdownContent
                    {
                        IsMarkdown = true,
                        Value      = description
                    }
                }
            );
        }

        return [..result];
    }

}
