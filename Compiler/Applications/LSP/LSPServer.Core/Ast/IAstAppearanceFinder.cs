using System.Collections.Generic;

using KSPCompiler.Commons.Text;
using KSPCompiler.Domain.Ast.Nodes.Blocks;

namespace KSPCompiler.Applications.LSPServer.Core.Ast;

public interface IAstAppearanceFinder
{
    IReadOnlyCollection<Position> Find( AstCompilationUnitNode ast );
}
