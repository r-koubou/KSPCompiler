using KSPCompiler.Interactors.ApplicationServices.Compilation;

namespace KSPCompiler.Applications.LSServer.LanguageServerFramework.Compilation;

public sealed class CompilationController( CompilationApplicationService service )
{
    private readonly CompilationApplicationService service = service;
}
