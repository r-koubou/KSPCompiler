using System;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Commons;

public sealed class NotSupportedVersionException : Exception
{
    public NotSupportedVersionException( int version, int expectedVersion )
        : base( $"Unsupported version: {version}, expected: {expectedVersion}" ) {}
}
