using System;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Yaml.Callbacks.Models;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Callbacks.Translators;

public class ToCallbackSymbolModelTranslator : IDataTranslator<CallbackSymbol, CallBackSymbolModel>
{
    public CallBackSymbolModel Translate( CallbackSymbol source )
    {
        var symbol = new CallBackSymbolModel
        {
            Id                       = Guid.NewGuid(),
            CreatedAt                = DateTime.UtcNow,
            UpdatedAt                = DateTime.UtcNow,
            Name                     = source.Name.Value,
            BuiltIn                  = source.BuiltIn,
            AllowMultipleDeclaration = source.AllowMultipleDeclaration,
            Description              = source.Description.Value,
            BuiltIntoVersion         = source.BuiltIntoVersion
        };

        foreach( var arg in source.Arguments )
        {
            var argument = new CallbackArgumentModel
            {
                Name            = arg.Name,
                RequiredDeclare = arg.RequiredDeclareOnInit,
                Description     = arg.Description
            };

            symbol.Arguments.Add( argument );
        }

        return symbol;
    }
}
