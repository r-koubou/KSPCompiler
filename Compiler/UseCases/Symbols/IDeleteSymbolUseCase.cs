using System;

using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.UseCases.Symbols;

public sealed class DeleteSymbolInputData<TSymbol> : IInputPort<Func<TSymbol, bool>> where TSymbol : SymbolBase
{
    public Func<TSymbol, bool> InputData { get; }

    public DeleteSymbolInputData( Func<TSymbol, bool> inputData )
    {
        InputData = inputData;
    }
}

public sealed class DeleteOutputData : IOutputPort<DeleteOutputDetail>
{
    public bool Result { get; }
    public Exception? Error { get; }
    public DeleteOutputDetail OutputData { get; }

    public DeleteOutputData( bool result, Exception? error, DeleteOutputDetail outputData )
    {
        Result     = result;
        Error      = error;
        OutputData = outputData;
    }
}

public sealed class DeleteOutputDetail
{
    public int DeletedCount { get; }

    public DeleteOutputDetail( int deletedCount )
    {
        DeletedCount = deletedCount;
    }
}

public interface IDeleteSymbolUseCase<TSymbol> : IUseCase<DeleteSymbolInputData<TSymbol>, DeleteOutputData> where TSymbol : SymbolBase {}
