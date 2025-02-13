using System.IO;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Gateways.EventEmitting;
using KSPCompiler.Infrastructures.Parser.Antlr;

using NUnit.Framework;

namespace KSPCompiler.Parser.Antlr.Tests.Commons;

public static class ParseTestUtility
{
    public static AstCompilationUnitNode Parse( string scriptDirectoryPath, string scriptFilePath )
    {
        var path = Path.Combine(
            TestContext.CurrentContext.TestDirectory,
            scriptDirectoryPath,
            scriptFilePath
        );

        var parser = new AntlrKspFileSyntaxParser( path, NullIEventEmitter.Instance );
        var ast = parser.Parse();
        return ast;
    }
}
