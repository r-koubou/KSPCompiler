using System;
using System.Collections.Generic;

using KSPCompiler.Shared.IO.Symbols.Yaml.Callbacks.Models;

using NUnit.Framework;

using YamlDotNet.Serialization;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.SymbolRepository.Yaml.Tests;

[TestFixture]
public class YamlDotNetDemoTest
{
    [Test]
    public void SerializeTest()
    {
        var symbol = new CallBackSymbolModel
        {
            Id                       = Guid.NewGuid(),
            Name                     = "Test",
            BuiltIn                  = true,
            AllowMultipleDeclaration = true,
            Description              = "Test Description",
            BuiltIntoVersion         = "1.0.0",
            Arguments = new List<CallbackArgumentModel>
            {
                new()
                {
                    Name            = "arg1",
                    RequiredDeclare = true,
                    Description     = "arg1 description\r## Note\naaaa."
                },
                new()
                {
                    Name            = "arg2",
                    RequiredDeclare = false,
                    Description     = "arg2 description"
                }
            }
        };

        var yaml = new SerializerBuilder()
                  .Build().Serialize( new List<CallBackSymbolModel>{ symbol} );

        Console.WriteLine( yaml );

        var symbols = new DeserializerBuilder()
                  .Build().Deserialize<List<CallBackSymbolModel>>( yaml );

        Console.WriteLine( symbols[ 0 ].Arguments[ 0 ].Description );
    }
}
