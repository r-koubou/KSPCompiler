using System.IO;
using System.Text;

using KSPCompiler.Domain.Ast.Node.Blocks;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Infrastructures.Parser.Antlr;

using NUnit.Framework;

namespace KSPCompiler.Parser.Antlr.Tests.Commons;

public static class ParseTestUtility
{
    public static AstCompilationUnit Parse( string scriptDirectoryPath, string scriptFilePath )
    {
        var path = Path.Combine(
            TestContext.CurrentContext.TestDirectory,
            scriptDirectoryPath,
            scriptFilePath
        );

        var p = new AntlrKspFileSyntaxAnalyzer( path, ICompilerMessageManger.CreateDefault(), Encoding.UTF8 );
        var result = p.Analyze();
        return result;
    }
}
