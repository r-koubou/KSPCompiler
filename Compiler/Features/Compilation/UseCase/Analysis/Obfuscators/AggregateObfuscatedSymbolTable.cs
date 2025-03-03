using KSPCompiler.UseCases.Analysis.Obfuscators;

namespace KSPCompiler.Interactors.Analysis.Obfuscators;

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
