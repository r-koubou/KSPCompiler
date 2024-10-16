using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Infrastructures.Parser.Antlr.Translators.Extensions;

namespace KSPCompiler.Infrastructures.Parser.Antlr.Translators
{
    // implementation of the statement node
    public partial class CstConverterVisitor
    {
        public override AstNode VisitKspPreprocessorDefine( KSPParser.KspPreprocessorDefineContext context )
        {
            var node = new AstKspPreprocessorDefineNode();

            node.Import( context );
            node.Symbol.Import( context.symbol );
            node.Symbol.Name = context.symbol.Text;

            return node;
        }

        public override AstNode VisitKspPreprocessorUndefine( KSPParser.KspPreprocessorUndefineContext context )
        {
            var node = new AstKspPreprocessorUndefineNode();

            node.Import( context );
            node.Symbol.Import( context.symbol );
            node.Symbol.Name = context.symbol.Text;

            return node;
        }

        public override AstNode VisitKspPreprocessorIfdefine( KSPParser.KspPreprocessorIfdefineContext context )
        {
            var node = new AstKspPreprocessorIfdefineNode();
            var block = context.block();

            node.Import( context );
            node.Condition.Import( context.symbol );
            node.Condition.Name = context.symbol.Text;

            if( block != null )
            {
                var b = context.block().Accept( this ) as AstBlockNode;
                _ = b ?? throw new MustBeNotNullException( nameof( b ) );

                b.Parent = node;
                node.Block = b;
            }

            return node;
        }

        public override AstNode VisitKspPreprocessorIfnotDefine( KSPParser.KspPreprocessorIfnotDefineContext context )
        {
            var node = new AstKspPreprocessorIfnotDefineNode();
            var block = context.block();

            node.Import( context );
            node.Condition.Import( context.symbol );
            node.Condition.Name = context.symbol.Text;

            if( block == null )
            {
                return node;
            }

            var b = context.block().Accept( this ) as AstBlockNode;
            _ = b ?? throw new MustBeNotNullException( nameof( b ) );

            b.Parent   = node;
            node.Block = b;

            return node;
        }
    }
}
