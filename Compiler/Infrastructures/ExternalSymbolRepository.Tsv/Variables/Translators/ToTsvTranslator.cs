using System.Collections.Generic;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.ExternalSymbolRepository.Tsv.Variables.Translators;

internal class ToTsvTranslator : IDataTranslator<ISymbolTable<VariableSymbol>, IReadOnlyCollection<string>>
{
    public IReadOnlyCollection<string> Translate( ISymbolTable<VariableSymbol> source )
    {
        var result = new List<string>();

        foreach( var (_, v) in source.Table )
        {
            result.Add( $"{v.Name}\t{v.Reserved.ToString().ToLower()}\t${v.Description}" );
        }

        return result;
    }
}
