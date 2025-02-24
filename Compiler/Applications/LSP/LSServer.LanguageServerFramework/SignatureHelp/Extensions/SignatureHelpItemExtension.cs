using System.Collections.Generic;

using KSPCompiler.Applications.LSPServer.CoreNew.SignatureHelp;

using FrameworkSignatureHelp = EmmyLua.LanguageServer.Framework.Protocol.Message.SignatureHelp.SignatureHelp;
using FrameworkSignatureInformation = EmmyLua.LanguageServer.Framework.Protocol.Message.SignatureHelp.SignatureInformation;
using FrameworkParameterInformation = EmmyLua.LanguageServer.Framework.Protocol.Message.SignatureHelp.ParameterInformation;
using FrameworkStringOrMarkupContent = EmmyLua.LanguageServer.Framework.Protocol.Model.Union.StringOrMarkupContent;

namespace KSPCompiler.Applications.LSServer.LanguageServerFramework.SignatureHelp.Extensions;

public static class SignatureHelpItemExtension
{
    public static FrameworkSignatureHelp As( this SignatureHelpItem self )
    {
        return new FrameworkSignatureHelp
        {
            Signatures = self.Signatures.As()
        };
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static FrameworkSignatureInformation As( this SignatureHelpInformationItem self )
    {
        FrameworkStringOrMarkupContent? documentation = null;
        uint? activaParameter = null;

        if( self.Documentation != null )
        {
            documentation = new FrameworkStringOrMarkupContent( self.Documentation.Value );
        }

        if( self.ActiveParameter != null )
        {
            activaParameter = (uint)self.ActiveParameter.Value;
        }

        return new FrameworkSignatureInformation
        {
            Label         = self.Label,
            Documentation = documentation,
            Parameters    = self.Parameters.As(),
            ActiveParameter = activaParameter,
        };
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static List<FrameworkSignatureInformation> As( this IEnumerable<SignatureHelpInformationItem> self )
    {
        var result = new List<FrameworkSignatureInformation>();

        foreach( var x in self )
        {
            result.Add( x.As() );
        }

        return result;
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static FrameworkParameterInformation As( this SignatureHelpParameterItem self )
    {
        FrameworkStringOrMarkupContent? documentation = null;

        if( self.Documentation != null )
        {
            documentation = new FrameworkStringOrMarkupContent( self.Documentation.Value );
        }

        return new FrameworkParameterInformation
        {
            Label         = self.Label,
            Documentation = documentation
        };
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static List<FrameworkParameterInformation> As( this IEnumerable<SignatureHelpParameterItem> self )
    {
        var result = new List<FrameworkParameterInformation>();

        foreach( var x in self )
        {
            result.Add( x.As() );
        }

        return result;
    }
}
