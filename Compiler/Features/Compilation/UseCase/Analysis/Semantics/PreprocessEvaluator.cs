using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Preprocessing;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Statements;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics;

public class PreprocessEvaluator : IPreprocessEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstPreprocessorDefineNode node )
        // 意味化石フェーズでは評価項目なし
        => node;

    public IAstNode Evaluate( IAstVisitor visitor, AstPreprocessorUndefineNode node )
        // 意味化石フェーズでは評価項目なし
        => node;

    public IAstNode Evaluate( IAstVisitor visitor, AstPreprocessorIfdefineNode node )
    {
        if( !node.Ignore )
        {
            node.Block.AcceptChildren( visitor );
        }

        return node;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstPreprocessorIfnotDefineNode node )
    {
        if( !node.Ignore )
        {
            node.Block.AcceptChildren( visitor );
        }

        return node;
    }
}
