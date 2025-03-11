using KSPCompiler.Shared.Mediator;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions;

public interface ICompilationMediator : IMediator<CompilationRequest, CompilationResponse>;
