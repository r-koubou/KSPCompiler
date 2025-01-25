using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.ExternalSymbolRepository.Yaml.Callbacks.Models;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Callbacks.Translators;

public class FromCallbackSymbolModelTranslator : IDataTranslator<CallBackSymbolModel, CallbackSymbol>
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
