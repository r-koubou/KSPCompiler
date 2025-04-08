using System;

using KSPCompiler.Features.Compilation.Gateways.EventEmitting;
using KSPCompiler.Resources;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.EventEmitting;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics.Extensions;

public static class SymbolTableExtension
{
    public static void EmitUnusedSymbol<TSymbol>(
        this ISymbolTable<TSymbol> self,
        IEventEmitter emitter,
        Predicate<TSymbol>? predicate = null
    ) where TSymbol : SymbolBase
    {
        predicate ??= x => x.State != SymbolState.Loaded;

        foreach( var symbol in self )
        {
            if( !predicate( symbol ) )
            {
                continue;
            }

            emitter.Emit(
                new CompilationInfoEvent(
                    string.Format( CompilerMessageResources.semantic_warning_unused, symbol.Name.Value ),
                    symbol.DefinedPosition.BeginLine.Value,
                    symbol.DefinedPosition.BeginColumn.Value
                )
            );
        }
    }
}
