using KSPCompiler.Features.Compilation.Gateways.Parser;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.EventEmitting;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions;

public sealed record CompilationRequest(
    ISyntaxParser SyntaxParser,
    AggregateSymbolTable BuiltinSymbolTable,
    IEventEmitter EventEmitter,
    bool EnableObfuscation
);
