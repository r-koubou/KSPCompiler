using System.Collections.Generic;

using KSPCompiler.Features.Compilation.Domain.Symbols;
using KSPCompiler.Features.Compilation.UseCase.Abstractions.Obfuscators;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;

public abstract class ObfuscatedSymbolTable<TSymbol> : IObfuscatedTable<TSymbol> where TSymbol : SymbolBase
{
    // <original name, obfuscated name>
    private Dictionary<string, string> ObfuscatedTable { get; } = new();

    private string Prefix { get; }

    private IObfuscateFormatter Formatter { get; }

    protected ObfuscatedSymbolTable( ISymbolTable<TSymbol> source, string prefix )
        : this( source, prefix, new DefaultObfuscateFormatter() ) {}

    protected ObfuscatedSymbolTable( ISymbolTable<TSymbol> source, string prefix, IObfuscateFormatter formatter )
    {
        Prefix    = prefix;
        Formatter = formatter;

        var generator = new UniqueSymbolIndexGenerator();
        Obfuscate( source, generator );
    }

    private void Obfuscate( ISymbolTable<TSymbol> source, UniqueSymbolIndexGenerator generator )
    {
        foreach( var (name, symbol) in source.Table )
        {
            Obfuscate( name, symbol, generator );
        }
    }

    private void Obfuscate( string name, TSymbol symbol, UniqueSymbolIndexGenerator generator )
    {
        var typePrefix = string.Empty;

        if( symbol.BuiltIn )
        {
            return;
        }

        if( KspRegExpConstants.TypePrefix.IsMatch( name ) )
        {
            typePrefix = name[ 0 ].ToString();
        }

        var obfuscatedName = $"{typePrefix}{Formatter.Format( name, Prefix, generator.Next() )}";
        ObfuscatedTable.TryAdd( name, obfuscatedName );
    }

    public bool TryGetObfuscatedByName( string original, out string result )
    {
        result = string.Empty;

        if( !ObfuscatedTable.TryGetValue( original, out var obfuscated ) )
        {
            return false;
        }

        result = obfuscated;
        return true;
    }

    public string GetObfuscatedByName( string original )
        => !TryGetObfuscatedByName( original, out var result ) ? original : result;
}
