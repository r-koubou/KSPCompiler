using System.Collections.Generic;
using System.Linq;

using KSPCompiler.Domain.Symbols;

using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.LSPServer.Core.SignatureHelps;

public class DefaultSignatureHelpBuilder : ISignatureHelpBuilder<CommandSymbol>
{
    public SignatureHelp Build( CommandSymbol symbol, int activeParameter )
    {
        return new()
        {
            Signatures = new Container<SignatureInformation>( BuildInformation( symbol, activeParameter ) ),
        };
    }

    private static SignatureInformation BuildInformation( CommandSymbol symbol, int activeParameter )
    {
        var label = string.Empty;

        if( symbol.ArgumentCount > 0 )
        {
            label = $"{symbol.Name.Value}({string.Join( ", ", symbol.Arguments.Select( arg => arg.Name.Value ) )})";
        }

        return new()
        {
            Label = label,
            Documentation = new StringOrMarkupContent(
                new MarkupContent
                {
                    Kind  = MarkupKind.Markdown,
                    Value = symbol.Description.Value
                }
            ),
            Parameters = BuildParameterInformation( symbol ),
            ActiveParameter = activeParameter
        };
    }

    private static Container<ParameterInformation>? BuildParameterInformation( CommandSymbol symbol )
    {
        if( symbol.ArgumentCount == 0 )
        {
            return null;
        }

        var result = new List<ParameterInformation>();

        foreach( var arg in symbol.Arguments )
        {
            if( string.IsNullOrEmpty( arg.Description ) )
            {
                continue;
            }

            result.Add(
                new ParameterInformation
                {
                    Label =　arg.Name.Value,
                    Documentation = new StringOrMarkupContent(
                        new MarkupContent
                        {
                            Kind  = MarkupKind.Markdown,
                            Value = arg.Description.Value
                        }
                    )
                }
            );
        }

        return new Container<ParameterInformation>( result );
    }

}
