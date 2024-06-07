using System.Collections.Generic;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Yaml.Callbacks.Model;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Callbacks.Translators;

internal class ToYamlTranslator : IDataTranslator<IEnumerable<CallbackSymbol>, RootObject>
{
    public RootObject Translate( IEnumerable<CallbackSymbol> source )
    {
        var result = new RootObject();

        foreach( var x in source )
        {
            var symbol = new Symbol
            {
                Name        = x.Name.Value,
                Reserved    = x.Reserved,
                AllowMultipleDeclaration  = x.AllowMultipleDeclaration,
                Description = x.Description.Value
            };

            foreach( var arg in x.Arguments )
            {
                var argument = new Argument
                {
                    Name        = arg.Name,
                    RequiredDeclare = arg.RequiredDeclareOnInit,
                    Description = arg.Description
                };

                symbol.Arguments.Add( argument );
            }

            result.Symbols.Add( symbol );
        }

        return result;
    }
}
