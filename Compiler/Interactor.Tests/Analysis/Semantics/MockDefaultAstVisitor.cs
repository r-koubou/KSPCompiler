using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Interactor.Tests.Analysis.Semantics;

/// <summary>
/// Mock visitor for just returning the node itself.
/// </summary>
public sealed class MockDefaultAstVisitor : DefaultAstVisitor {}
