using System.IO;
using System.Text;

using KSPCompiler.Domain.CompilerMessages;

namespace KSPCompiler.Infrastructures.Parser.Antlr;

public class AntlrKspFileSyntaxAnalyzer : AntlrKspSyntaxAnalyzer
{
    // ReSharper disable once UnusedParameter.Local
    public AntlrKspFileSyntaxAnalyzer( string path, ICompilerMessageManger messageManger, Encoding _ )
        : base( File.OpenRead( path ), messageManger )
    {}
}
