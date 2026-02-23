using System.IO;

using KSPCompiler.Shared.Domain.Compilation.Symbols;

using NUnit.Framework;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.SymbolRepository.Yaml.Tests;

public static class MockUtility
{
    public static string GetTestDataDirectory( string name )
    {
        return Path.Combine(
            TestContext.CurrentContext.TestDirectory,
            "TestData",
            name
        );
    }

    public static CallbackSymbol CreateCallbackSymbolModel(string name)
    {
        return new CallbackSymbol( true )
        {
            Name             = name,
            Description      = $"{name} Description",
            BuiltIn          = true,
            BuiltIntoVersion = "1.0.0",
            Arguments =
            {
                new CallbackArgumentSymbol( true )
                {
                    Name        = "arg1",
                    Description = "arg1 description\r## Note\naaaa.",
                }
            }
        };
    }
}
