using KSPCompiler.Features.Compilation.Gateways.EventEmitting;
using KSPCompiler.Features.Compilation.Infrastructures.Parser.Antlr.Translators.Extensions;
using KSPCompiler.Infrastructures.Parser.Antlr;
using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Shared.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Ast.Nodes.Statements;
using KSPCompiler.Shared.Domain.Symbols.MetaData;

namespace KSPCompiler.Features.Compilation.Infrastructures.Parser.Antlr.Translators
{
    // implementation of the statement node
    public partial class CstConverterVisitor
    {
        public override AstNode VisitPreprocessorDefine( KSPParser.PreprocessorDefineContext context )
        {
            var node = new AstPreprocessorDefineNode();

            node.Import( tokenStream, context );

            if( context.symbol != null )
            {
                node.Symbol.Import( tokenStream, context.symbol );
                node.Symbol.Name = context.symbol.Text;
            }
            else
            {
                eventEmitter.Emit( new LogDebugEvent( $"{nameof( VisitPreprocessorDefine )} fallback" ) );
            }

            return node;
        }

        public override AstNode VisitPreprocessorUndefine( KSPParser.PreprocessorUndefineContext context )
        {
            var node = new AstPreprocessorUndefineNode();

            if( context.symbol != null )
            {
                node.Symbol.Import( tokenStream, context.symbol );
                node.Symbol.Name = context.symbol.Text;
            }
            else
            {
                eventEmitter.Emit( new LogDebugEvent( $"{nameof( VisitPreprocessorUndefine )} fallback" ) );
            }

            return node;
        }

        public override AstNode VisitPreprocessorIfdefine( KSPParser.PreprocessorIfdefineContext context )
        {
            var node = new AstPreprocessorIfdefineNode();
            var symbol = new AstSymbolExpressionNode
            {
                Parent   = node,
                TypeFlag = DataTypeFlag.TypePreprocessorSymbol
            };

            if( context.symbol != null )
            {
                symbol.Import( tokenStream, context.symbol );
                symbol.Name = context.symbol.Text;
            }
            else
            {
                eventEmitter.Emit( new LogDebugEvent( $"{nameof( VisitPreprocessorIfdefine )} fallback" ) );
            }

            var block = context.block();


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
            var symbol = new AstSymbolExpressionNode()
            {
                Parent   = node,
                TypeFlag = DataTypeFlag.TypePreprocessorSymbol
            };

            if( context.symbol != null )
            {
                symbol.Import( tokenStream, context.symbol );
                symbol.Name = context.symbol.Text;
            }
            else
            {
                eventEmitter.Emit( new LogDebugEvent( $"{nameof( VisitPreprocessorIfnotDefine )} fallback" ) );
            }

            var block = context.block();

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
