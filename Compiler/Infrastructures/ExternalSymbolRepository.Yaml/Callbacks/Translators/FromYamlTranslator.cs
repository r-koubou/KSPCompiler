using System.Collections.Generic;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.ExternalSymbolRepository.Yaml.Callbacks.Model;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Callbacks.Translators;

internal class FromYamlTranslator : IDataTranslator<RootObject, IReadOnlyCollection<CallbackSymbol>>
{
    public IReadOnlyCollection<CallbackSymbol> Translate( RootObject source )
    {
        var result = new List<CallbackSymbol>();

        foreach( var x in source.Symbols )
        {
            var command = new CallbackSymbol( x.AllowMultipleDeclaration )
            {
                Name        = x.Name,
                Reserved    = x.Reserved,
                Description = x.Description,
                DataType    = DataTypeFlag.None
            };

            foreach( var arg in x.Arguments )
            {
                var argument = new CallbackArgumentSymbol( arg.RequiredDeclare )
                {
                    Name        = arg.Name,
                    Description = arg.Description,
                    Reserved    = false
                };

                argument.DataType = DataTypeUtility.Guess( argument.Name );
                command.AddArgument( argument );
            }

            result.Add( command );
        }

        return result;
    }
}
