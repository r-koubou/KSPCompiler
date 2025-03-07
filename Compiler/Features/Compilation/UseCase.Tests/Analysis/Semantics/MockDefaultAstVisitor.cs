using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Semantics;

/// <summary>
/// Mock visitor for just returning the node itself.
/// </summary>
public sealed class MockDefaultAstVisitor : DefaultAstVisitor {}
