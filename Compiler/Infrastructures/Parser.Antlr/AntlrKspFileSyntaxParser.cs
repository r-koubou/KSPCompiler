using System.IO;

using KSPCompiler.Domain.Events;

namespace KSPCompiler.Infrastructures.Parser.Antlr;

public class AntlrKspFileSyntaxParser : AntlrKspSyntaxParser
{
    // ReSharper disable once UnusedParameter.Local
    public AntlrKspFileSyntaxParser( string path, IEventEmitter eventEmitter )
        : base( File.OpenRead( path ), eventEmitter )
    {}
}
