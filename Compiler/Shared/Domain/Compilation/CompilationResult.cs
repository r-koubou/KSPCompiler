using System;

using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks;
using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Shared.Domain.Compilation;

public sealed record CompilationResult(
    bool Result,
    Exception? Error,
    AstCompilationUnitNode? Ast,
    AggregateSymbolTable SymbolTable,
    string ObfuscatedScript
);
