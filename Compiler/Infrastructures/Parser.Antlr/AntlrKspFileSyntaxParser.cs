using System.IO;

using KSPCompiler.Domain.Events;

namespace KSPCompiler.Infrastructures.Parser.Antlr;

public class AntlrKspFileSyntaxParser : AntlrKspSyntaxParser
{
    // ReSharper disable once UnusedParameter.Local
    public AntlrKspFileSyntaxParser( string path, IEventDispatcher eventDispatcher )
        : base( File.OpenRead( path ), eventDispatcher )
    {}
}
