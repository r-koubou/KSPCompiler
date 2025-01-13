using System.Collections.Generic;

using KSPCompiler.Commons.Text;
using KSPCompiler.Domain.Ast.Nodes.Blocks;

namespace KSPCompiler.LSPServer.Core.Ast;

public interface ISymbolAppearanceFinder
{
    IReadOnlyCollection<Position> Find( AstCompilationUnitNode ast );
}
