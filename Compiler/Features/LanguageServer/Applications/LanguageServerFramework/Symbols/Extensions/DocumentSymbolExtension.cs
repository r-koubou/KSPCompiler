using System.Collections.Generic;

using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Extensions;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Symbol;

using FrameworkDocumentSymbol = EmmyLua.LanguageServer.Framework.Protocol.Message.DocumentSymbol.DocumentSymbol;
using FrameworkSymbolKind = EmmyLua.LanguageServer.Framework.Protocol.Message.DocumentSymbol.SymbolKind;

namespace KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Symbols.Extensions;

public static class DocumentSymbolExtension
{
    public static FrameworkDocumentSymbol As( this DocumentSymbol self )
    {
        return new FrameworkDocumentSymbol
        {
            Name           = self.Name,
            Detail         = self.Detail,
            Kind           = self.Kind.As(),
            Range          = self.Range.AsRange(),
            SelectionRange = self.SelectionRange.AsRange(),
        };
    }

    public static List<FrameworkDocumentSymbol> As( this IEnumerable<DocumentSymbol> self )
    {
        var result = new List<FrameworkDocumentSymbol>();

        foreach( var x in self )
        {
            result.Add( x.As() );
        }

        return result;
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static FrameworkSymbolKind As( this SymbolKind self )
    {
        return (FrameworkSymbolKind)(int)self;
    }
}
