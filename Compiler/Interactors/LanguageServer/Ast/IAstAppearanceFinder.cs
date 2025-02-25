using System.Collections.Generic;

using KSPCompiler.Commons.Text;
using KSPCompiler.Domain.Ast.Nodes.Blocks;

namespace KSPCompiler.Interactors.LanguageServer.Ast;

public interface IAstAppearanceFinder
{
    IReadOnlyCollection<Position> Find( AstCompilationUnitNode ast );
}
