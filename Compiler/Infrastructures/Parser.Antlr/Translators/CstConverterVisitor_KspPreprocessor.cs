using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Infrastructures.Parser.Antlr.Translators.Extensions;

namespace KSPCompiler.Infrastructures.Parser.Antlr.Translators
{
    // implementation of the statement node
    public partial class CstConverterVisitor
    {
        public override AstNode VisitPreprocessorDefine( KSPParser.PreprocessorDefineContext context )
        {
            var node = new AstPreprocessorDefineNode();

            node.Import( tokenStream, context );
            node.Symbol.Import( tokenStream, context.symbol );
            node.Symbol.Name = context.symbol.Text;

            return node;
        }

        public override AstNode VisitPreprocessorUndefine( KSPParser.PreprocessorUndefineContext context )
        {
            var node = new AstPreprocessorUndefineNode();

            node.Import( tokenStream, context );
            node.Symbol.Import( tokenStream, context.symbol );
            node.Symbol.Name = context.symbol.Text;

            return node;
        }

        public override AstNode VisitPreprocessorIfdefine( KSPParser.PreprocessorIfdefineContext context )
        {
            var node = new AstPreprocessorIfdefineNode();
            var symbol = new AstSymbolExpressionNode( context.symbol.Text )
            {
                Parent   = node,
                TypeFlag = DataTypeFlag.TypePreprocessorSymbol
            };
            var block = context.block();

            symbol.Import( tokenStream, context.symbol );

            node.Import( tokenStream, context );
            node.Condition = symbol;

            if( block == null )
            {
                return node;
            }

            var b = context.block().Accept( this ) as AstBlockNode
                    ?? NullAstBlockNode.Instance;

            b.Parent   = node;
            node.Block = b;

            return node;
        }

        public override AstNode VisitPreprocessorIfnotDefine( KSPParser.PreprocessorIfnotDefineContext context )
        {
            var node = new AstPreprocessorIfnotDefineNode();
            var symbol = new AstSymbolExpressionNode( context.symbol.Text )
            {
                Parent   = node,
                TypeFlag = DataTypeFlag.TypePreprocessorSymbol
            };
            var block = context.block();

            symbol.TypeFlag = DataTypeFlag.TypePreprocessorSymbol;
            symbol.Parent   = node;
            symbol.Import( tokenStream, context.symbol );

            node.Import( tokenStream, context );
            node.Condition = symbol;

            if( block == null )
            {
                return node;
            }

            var b = context.block().Accept( this ) as AstBlockNode
                ?? NullAstBlockNode.Instance;

            b.Parent   = node;
            node.Block = b;

            return node;
        }
    }
}
