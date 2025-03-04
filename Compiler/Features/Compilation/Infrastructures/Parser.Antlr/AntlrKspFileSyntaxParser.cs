using System.IO;

using KSPCompiler.Shared.EventEmitting;

namespace KSPCompiler.Features.Compilation.Infrastructures.Parser.Antlr;

public class AntlrKspFileSyntaxParser : AntlrKspSyntaxParser
{
    // ReSharper disable once UnusedParameter.Local
    public AntlrKspFileSyntaxParser( string path, IEventEmitter eventEmitter )
        : base( File.OpenRead( path ), eventEmitter )
    {}
}
