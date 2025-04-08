using System.Text;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Preprocessing;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators.Extensions;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Statements;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;

public class PreprocessEvaluator : IPreprocessEvaluator
{
    private StringBuilder Output { get; }

    public PreprocessEvaluator( StringBuilder output )
    {
        Output = output;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstPreprocessorDefineNode node )
    {
        Output.Append( "SET_CONDITION(" );

        node.Symbol.Accept( visitor );

        Output.Append( ')' )
              .NewLine();

        return node;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstPreprocessorUndefineNode node )
    {
        Output.Append( "RESET_CONDITION(" );

        node.Symbol.Accept( visitor );

        Output.Append( ')' )
              .NewLine();

        return node;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstPreprocessorIfdefineNode node )
    {
        // 暫定的にシュリンクせずにそのまま出力する
        // if( node.Ignore )
        // {
        //     return node;
        // }

        Output.Append( "USE_CODE_IF(" );

        node.Condition.Accept( visitor );

        Output.Append( ')' )
              .NewLine();

        node.Block.AcceptChildren( visitor );

        Output.Append( "END_USE_CODE" )
              .NewLine();

        return node;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstPreprocessorIfnotDefineNode node )
    {
        // 暫定的にシュリンクせずにそのまま出力する
        // if( node.Ignore )
        // {
        //     return node;
        // }

        Output.Append( "USE_CODE_IF_NOT(" );

        node.Condition.Accept( visitor );

        Output.Append( ')' )
              .NewLine();

        node.Block.AcceptChildren( visitor );

        Output.Append( "END_USE_CODE" )
              .NewLine();

        return node;
    }
}
