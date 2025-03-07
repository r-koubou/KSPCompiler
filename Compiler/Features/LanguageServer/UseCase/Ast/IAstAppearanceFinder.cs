using System.Collections.Generic;

using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks;
using KSPCompiler.Shared.Text;

namespace KSPCompiler.Features.LanguageServer.UseCase.Ast;

public interface IAstAppearanceFinder
{
    IReadOnlyCollection<Position> Find( AstCompilationUnitNode ast );
}
