using System.IO;
using System.Text;

using KSPCompiler.Domain.Events;

namespace KSPCompiler.Infrastructures.Parser.Antlr;

public class AntlrKspStringSyntaxParser : AntlrKspSyntaxParser
{
    public AntlrKspStringSyntaxParser( string script, IEventEmitter eventEmitter, Encoding encoding )
        : base( new MemoryStream( encoding.GetBytes( script ) ), eventEmitter )
    {}
}
