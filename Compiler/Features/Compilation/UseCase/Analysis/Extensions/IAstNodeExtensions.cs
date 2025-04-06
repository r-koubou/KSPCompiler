using KSPCompiler.Features.Compilation.Gateways.EventEmitting;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Extensions;

public static class IAstNodeExtensions
{
    public static CompilationFatalEvent AsFatalEvent( this IAstNode self, string message, params object[] argv )
    {
        return new CompilationFatalEvent( string.Format( message, argv ), self.Position );
    }

    public static CompilationErrorEvent AsErrorEvent( this IAstNode self, string message, params object[] argv )
    {
        return new CompilationErrorEvent( string.Format( message, argv ), self.Position );
    }

    public static CompilationWarningEvent AsWarningEvent( this IAstNode self, string message, params object[] argv )
    {
        return new CompilationWarningEvent( string.Format( message, argv ), self.Position );
    }

    public static CompilationInfoEvent AsInfoEvent( this IAstNode self, string message, params object[] argv )
    {
        return new CompilationInfoEvent( string.Format( message, argv ), self.Position );
    }
}
