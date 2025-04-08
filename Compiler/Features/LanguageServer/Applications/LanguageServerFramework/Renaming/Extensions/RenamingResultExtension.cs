using System.Collections.Generic;

using EmmyLua.LanguageServer.Framework.Protocol.Model.TextEdit;

using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Extensions;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Renaming;

using RenamingResult = System.Collections.Generic.Dictionary<KSPCompiler.Features.LanguageServer.UseCase.Abstractions.ScriptLocation, System.Collections.Generic.List<KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Renaming.RenamingItem>>;
using FrameworkRenamingResult = System.Collections.Generic.Dictionary<EmmyLua.LanguageServer.Framework.Protocol.Model.DocumentUri, System.Collections.Generic.List<EmmyLua.LanguageServer.Framework.Protocol.Model.TextEdit.TextEdit>>;

namespace KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Renaming.Extensions;


public static class RenamingResultExtension
{
    public static FrameworkRenamingResult As( this RenamingResult self )
    {
        var result = new FrameworkRenamingResult();

        foreach( var (key, value) in self )
        {
            var textEdits = new List<TextEdit>();

            foreach( var item in value )
            {
                textEdits.Add( item.As() );
            }

            result.Add( key.AsDocumentUri(), textEdits );
        }

        return result;
    }

    public static TextEdit As( this RenamingItem self )
    {
        return new TextEdit
        {
            Range   = self.Range.AsRange(),
            NewText = self.NewText,
        };
    }
}
