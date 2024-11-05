using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Domain.Ast.Analyzers.Evaluators.Preprocessing;

public interface IPreprocessEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstPreprocessorIfdefineNode node );
    public IAstNode Evaluate( IAstVisitor visitor, AstPreprocessorIfnotDefineNode node );
}
