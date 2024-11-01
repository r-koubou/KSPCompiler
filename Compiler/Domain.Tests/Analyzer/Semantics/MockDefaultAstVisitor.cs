using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

/// <summary>
/// Mock visitor for just returning the node itself.
/// </summary>
public sealed class MockDefaultAstVisitor : DefaultAstVisitor {}
