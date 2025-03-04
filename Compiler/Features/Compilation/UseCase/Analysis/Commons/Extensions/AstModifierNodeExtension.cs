using System.Collections.Generic;
using System.Linq;

using KSPCompiler.Shared.Domain.Ast.Nodes;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Extensions;

public static class AstModifierNodeExtension
{
    public static bool HasConstant( this AstModiferNode self )
        => self.Values.Contains( "const" );

    public static bool HasPolyphonic( this AstModiferNode self )
        => self.Values.Contains( "polyphonic" );

    public static IReadOnlyCollection<string> GetUIModifiers( this AstModiferNode self )
        => self.Values
               .Where( x => x != "const" && x != "polyphonic" )
               .ToList();
}
