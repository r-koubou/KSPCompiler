using Antlr4.Runtime;

using KSPCompiler.Features.Compilation.Gateways.EventEmitting;

namespace KSPCompiler.Features.Compilation.Infrastructures.Parser.Antlr.Translators.Extensions;

public static class ParserRuleContextExtension
{
    public static CompilationDebugEvent AsDebugEvent( this ParserRuleContext context, string message )
    {
        return new CompilationDebugEvent( message, context.Start.Line, context.Start.Column );
    }

    public static CompilationInfoEvent AsInfoEvent( this ParserRuleContext context, string message )
    {
        return new CompilationInfoEvent( message, context.Start.Line, context.Start.Column );
    }

    public static CompilationWarningEvent AsWarningEvent( this ParserRuleContext context, string message )
    {
        return new CompilationWarningEvent( message, context.Start.Line, context.Start.Column );
    }

    public static CompilationErrorEvent AsErrorEvent( this ParserRuleContext context, string message )
    {
        return new CompilationErrorEvent( message, context.Start.Line, context.Start.Column );
    }

    public static CompilationFatalEvent AsFatalEvent( this ParserRuleContext context, string message )
    {
        return new CompilationFatalEvent( message, context.Start.Line, context.Start.Column );
    }
}
