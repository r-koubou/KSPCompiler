using System;

using KSPCompiler.ExternalSymbolRepository.Yaml.Variables.Model;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Variables;

public class NotSupportedVersionException : Exception
{
    public NotSupportedVersionException( RootObject rootObject )
        : base( $"Unsupported version: {rootObject.Version}, expected: {RootObject.CurrentVersion}" ) {}
}
