namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;

public sealed class DefaultObfuscateFormatter : IObfuscateFormatter
{
    public string Format( string original, string prefix, UniqueSymbolIndex index )
        => $"{prefix}{index.Value}";
}
