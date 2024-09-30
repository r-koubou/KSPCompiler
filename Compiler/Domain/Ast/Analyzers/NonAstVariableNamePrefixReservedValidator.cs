using System.Linq;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Ast.Node.Statements;

namespace KSPCompiler.Domain.Ast.Analyzers;

/// <summary>
/// Validates that a variable name does not start with a reserved prefix.
/// </summary>
public class NonAstVariableNamePrefixReservedValidator : IDataValidator<AstVariableDeclaration>
{
    /// <summary>
    /// The prefix of a variable name that NI disallows to be used.
    /// </summary>
    private static readonly string[] NiReservedPrefix =
    {
        // From KSP Reference Manual:
        // Please do not create variables with the prefixes below, as these prefixes are used for
        // internal variables and constants
        "$NI_",
        "$CONTROL_PAR_",
        "$EVENT_PAR_",
        "$ENGINE_PAR_",
    };

    public bool Validate( AstVariableDeclaration data )
        => NiReservedPrefix.All( prefix => !data.Name.StartsWith( prefix ) );
}
