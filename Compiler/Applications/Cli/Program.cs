using System;

using KSPCompiler.Controllers.Compiler;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Infrastructures.Parser.Antlr;

namespace KSPCompiler.Applications.Cli;

public class Program
{
    public static void Main( string[] args )
    {
        var messageManager = ICompilerMessageManger.Default;
        var parser = new AntlrKspFileSyntaxParser( "example.ksp", messageManager );

        var controller = new CompilerController();
        controller.Execute( messageManager, new CompilerOption( parser ) );

        messageManager.WriteTo( Console.Out );
    }
}
