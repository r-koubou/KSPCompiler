using System;

namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commons;

public class InvalidJsonRepositoryTypeException : Exception
{
    public InvalidJsonRepositoryTypeException( string message ) : base( message ) {}
}
