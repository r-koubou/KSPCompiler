using System.Text;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

public static class StringBuilderExtensions
{
    public static void NewLine( this StringBuilder stringBuilder )
    {
        stringBuilder.Append( '\n' );
    }
}
