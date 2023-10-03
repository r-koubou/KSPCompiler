using System.IO;
using System.Text;

using KSPCompiler.Domain.CompilerMessages;

namespace KSPCompiler.Infrastructures.Parser.Antlr;

public class KspFileSyntaxAnalyser : KspSyntaxAnalyser
{
    // ReSharper disable once UnusedParameter.Local
    public KspFileSyntaxAnalyser( string path, ICompilerMessageManger messageManger, Encoding _ )
        : base( File.OpenRead( path ), messageManger )
    {}
}
