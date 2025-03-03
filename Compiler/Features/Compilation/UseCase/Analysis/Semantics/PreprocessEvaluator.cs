using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.UseCases.Analysis.Evaluations.Preprocessing;

namespace KSPCompiler.Interactors.Analysis.Semantics;

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
