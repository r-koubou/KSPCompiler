using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Preprocessing;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Statements;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.EventEmitting;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Preprocessing;

public class PreprocessAnalyzer( IPreProcessorSymbolTable symbolTable, IEventEmitter eventEmitter )
    : DefaultAstVisitor, IAstTraversal
{
    private IPreprocessEvaluator Evaluator { get; } = new PreprocessEvaluator( symbolTable, eventEmitter );

    public void Traverse( AstCompilationUnitNode node )
    {
        node.AcceptChildren( this );
    }

    public override IAstNode Visit( AstPreprocessorDefineNode node )
        => Evaluator.Evaluate( this, node );

    public override IAstNode Visit( AstPreprocessorUndefineNode node )
        => Evaluator.Evaluate( this, node );

    public override IAstNode Visit( AstPreprocessorIfdefineNode node )
        => Evaluator.Evaluate( this, node );

    public override IAstNode Visit( AstPreprocessorIfnotDefineNode node )
        => Evaluator.Evaluate( this, node );
}
