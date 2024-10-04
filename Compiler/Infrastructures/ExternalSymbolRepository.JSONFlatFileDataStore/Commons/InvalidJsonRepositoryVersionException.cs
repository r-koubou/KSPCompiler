using System;

namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commons;

public class InvalidJsonRepositoryVersionException : Exception
{
    public int ExpectedVersion { get; }
    public int ActualVersion { get; }

    public InvalidJsonRepositoryVersionException( int expectedVersion, int actualVersion ) : base( $"Invalid repository version. (expected: {expectedVersion}, actual: {actualVersion})" )
    {
        ExpectedVersion = expectedVersion;
        ActualVersion   = actualVersion;
    }
}
