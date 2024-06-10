using System.IO;
using System.Text;

using KSPCompiler.Domain.CompilerMessages;

namespace KSPCompiler.Infrastructures.Parser.Antlr;

public class KspFileSyntaxAnalyzer : KspSyntaxAnalyzer
{
    // ReSharper disable once UnusedParameter.Local
    public KspFileSyntaxAnalyzer( string path, ICompilerMessageManger messageManger, Encoding _ )
        : base( File.OpenRead( path ), messageManger )
    {}
}
