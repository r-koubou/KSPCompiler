using System;
using System.Text;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Preprocessing;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators.Extensions;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Statements;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Tests.Analysis.Obfuscators;

public class MockPreprocessEvaluatorVisitor : DefaultAstVisitor
{
    private StringBuilder Output { get; }
    private IPreprocessEvaluator Evaluator { get; set; } = new MockPreprocessEvaluator();

    public MockPreprocessEvaluatorVisitor( StringBuilder output )
    {
        Output = output;
    }

    public void Inject( IPreprocessEvaluator evaluator )
        => Evaluator = evaluator;

    public override IAstNode Visit( AstPreprocessorDefineNode node )
        => Evaluator.Evaluate( this, node );

    public override IAstNode Visit( AstPreprocessorUndefineNode node )
        => Evaluator.Evaluate( this, node );

    public override IAstNode Visit( AstPreprocessorIfdefineNode node )
        => Evaluator.Evaluate( this, node );

    public override IAstNode Visit( AstPreprocessorIfnotDefineNode node )
        => Evaluator.Evaluate( this, node );

    public override IAstNode Visit( AstSymbolExpressionNode node )
    {
        Output.Append( node.Name );
        return node;
    }

    public override IAstNode Visit( AstCallCommandExpressionNode node )
    {
        node.Left.Accept( this );

        Output.Append( "()" )
              .NewLine();

        return node;
    }

    private class MockPreprocessEvaluator : IPreprocessEvaluator
    {
        public IAstNode Evaluate( IAstVisitor visitor, AstPreprocessorDefineNode node )
            => throw new NotImplementedException();

        public IAstNode Evaluate( IAstVisitor visitor, AstPreprocessorUndefineNode node )
            => throw new NotImplementedException();

        public IAstNode Evaluate( IAstVisitor visitor, AstPreprocessorIfdefineNode node )
            => throw new NotImplementedException();

        public IAstNode Evaluate( IAstVisitor visitor, AstPreprocessorIfnotDefineNode node )
            => throw new NotImplementedException();
    }
}
