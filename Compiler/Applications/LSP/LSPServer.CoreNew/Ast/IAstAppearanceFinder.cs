using System.Collections.Generic;

using KSPCompiler.Commons.Text;
using KSPCompiler.Domain.Ast.Nodes.Blocks;

namespace KSPCompiler.Applications.LSPServer.CoreNew.Ast;

public interface IAstAppearanceFinder
{
    IReadOnlyCollection<Position> Find( AstCompilationUnitNode ast );
}
