using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Obfuscators;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;

public sealed class AggregateObfuscatedSymbolTable
{
    public IObfuscatedVariableTable Variables { get; }
    public IObfuscatedUserFunctionTable UserFunctions { get; }

    public AggregateObfuscatedSymbolTable(
        IObfuscatedVariableTable variables,
        IObfuscatedUserFunctionTable userFunctions )
    {
        Variables     = variables;
        UserFunctions = userFunctions;
    }
}
