using System.Linq;

using KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Statements;
using KSPCompiler.Shared;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations;

/// <summary>
/// Validates that a variable name does not start with a built-in prefix.
/// </summary>
public class NonAstVariableNameBuiltInValidator : IDataValidator<AstVariableDeclarationNode>
{
    /// <summary>
    /// The prefix of a variable name that NI disallows to be used.
    /// </summary>
    private static readonly string[] BuiltInPrefixList =
    {
        // From KSP Reference Manual:
        // Please do not create variables with the prefixes below, as these prefixes are used for
        // internal variables and constants
        "$NI_",
        "$CONTROL_PAR_",
        "$EVENT_PAR_",
        "$ENGINE_PAR_",
    };

    public bool Validate( AstVariableDeclarationNode data )
        => BuiltInPrefixList.All( prefix => !data.Name.StartsWith( prefix ) );
}
