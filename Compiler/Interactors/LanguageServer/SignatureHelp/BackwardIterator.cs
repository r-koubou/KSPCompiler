//--------------------------------------------------------------------------------------------------------
// Implemented based on Part of PHP Signature Help Provider implementation. (signatureHelpProvider.ts)
// https://github.com/microsoft/vscode/blob/main/extensions/php-language-features/src/features/signatureHelpProvider.ts
//--------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace KSPCompiler.Interactors.LanguageServer.SignatureHelp;

internal class BackwardIterator(
    IReadOnlyList<string> lines,
    int beginLine,
    int beginColumn
)
{
    private readonly IReadOnlyList<string> lines = lines;
    private int lineIndex = beginLine;
    private int columnIndex = beginColumn;

    public bool HasNext
        => lineIndex >= 0;

    public char GetNext()
    {
        if( columnIndex < 0 )
        {
            if( lineIndex > 0 )
            {

                lineIndex--;
                columnIndex = lines[ lineIndex ].Length - 1;

                return '\n';
            }

            lineIndex = -1;

            return '\0';
        }

        var c = lines[ lineIndex ][ columnIndex ];
        columnIndex--;

        return c;
    }
}
