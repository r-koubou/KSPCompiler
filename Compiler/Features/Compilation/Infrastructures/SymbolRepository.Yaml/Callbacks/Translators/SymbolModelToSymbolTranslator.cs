using KSPCompiler.Features.Compilation.Infrastructures.SymbolRepository.Yaml.Callbacks.Models;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;

namespace KSPCompiler.Features.Compilation.Infrastructures.SymbolRepository.Yaml.Callbacks.Translators;

public class SymbolModelToSymbolTranslator
    : ISymbolModelToSymbolTranslator<CallBackSymbolModel, CallbackSymbol>
{
    public CallbackSymbol Translate( CallBackSymbolModel source )
    {
        var model = new CallbackSymbol( source.AllowMultipleDeclaration )
        {
            Name             = source.Name,
            BuiltIn          = source.BuiltIn,
            Description      = source.Description,
            BuiltIntoVersion = source.BuiltIntoVersion
        };

        foreach( var arg in source.Arguments )
        {
            var argument = new CallbackArgumentSymbol( arg.RequiredDeclare )
            {
                Name        = arg.Name,
                Description = arg.Description,
                BuiltIn     = false
            };

            argument.DataType = DataTypeUtility.GuessFromSymbolName( arg.Name );
            model.Arguments.Add( argument );
        }

        return model;
    }
}
