using System;

using KSPCompiler.ExternalSymbolRepository.Yaml.Variables.v1.Model;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Variables.v1;

public class NotSupportedVersionException : Exception
{
    public NotSupportedVersionException( RootObject rootObject )
        : base( $"Unsupported version: {rootObject.Version}, expected: {RootObject.CurrentVersion}" ) {}
}
