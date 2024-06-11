using System.IO;
using System.Text;

using KSPCompiler.Domain.CompilerMessages;

namespace KSPCompiler.Infrastructures.Parser.Antlr;

public class AntlrKspFileSyntaxParser : AntlrKspSyntaxParser
{
    // ReSharper disable once UnusedParameter.Local
    public AntlrKspFileSyntaxParser( string path, ICompilerMessageManger messageManger, Encoding _ )
        : base( File.OpenRead( path ), messageManger )
    {}
}
