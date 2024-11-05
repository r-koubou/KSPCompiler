using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Preprocessing;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Domain.Ast.Analyzers.Semantics;

public class PreprocessEvaluator : IPreprocessEvaluator
{
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