using System.Collections.Generic;

using EmmyLua.LanguageServer.Framework.Protocol.Model.Union;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Completion;

using FrameworkCompletionItem = EmmyLua.LanguageServer.Framework.Protocol.Message.Completion.CompletionItem;
using FrameworkCompletionItemKind = EmmyLua.LanguageServer.Framework.Protocol.Message.Completion.CompletionItemKind;
using FrameworkInsertTextFormat = EmmyLua.LanguageServer.Framework.Protocol.Model.Kind.InsertTextFormat;

namespace KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Completion.Extensions;

public static class CompletionItemExtension
{
    public static FrameworkCompletionItem As( this CompletionItem self )
    {
        StringOrMarkupContent? documentation = null;

        if( self.Documentation != null )
        {
            documentation = new StringOrMarkupContent( self.Documentation );
        }

        return new FrameworkCompletionItem
        {
            Label            = self.Label,
            Kind             = (FrameworkCompletionItemKind)(int)self.Kind,
            Detail           = self.Detail,
            Documentation    = documentation,
            InsertText       = self.InsertText,
            InsertTextFormat = (FrameworkInsertTextFormat)self.InsertTextFormat
        };
    }

    public static List<FrameworkCompletionItem> As( this IEnumerable<CompletionItem> self )
    {
        var result = new List<FrameworkCompletionItem>();

        foreach( var x in self )
        {
            result.Add( x.As() );
        }

        return result;
    }
}
