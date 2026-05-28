using System;

namespace KSPCompiler.Shared;

public abstract class Result<TValue, TReason>
{
    public static Result<TValue, TReason> Success( TValue value )
        => new SuccessResult<TValue, TReason>( value );

    public static Result<TValue, TReason> Failure( TReason reason, Exception? error = null )
        => new FailureResult<TValue, TReason>( reason, error );

    public bool IsSuccess
        => this is SuccessResult<TValue, TReason>;

    public bool IsFailure
        => this is FailureResult<TValue, TReason>;

    public TReason Reason
        => this switch
        {
            FailureResult<TValue, TReason> f => f.Reason,
            SuccessResult<TValue, TReason>   => throw new InvalidOperationException( "Success has no reason." ),
            _                                => throw new InvalidOperationException( "Unknown reason." )
        };

    public Result<TOut, TReason> Map<TOut>( Func<TValue, TOut> success )
    {
        return this switch
        {
            SuccessResult<TValue, TReason> s
                => Result<TOut, TReason>.Success( success( s.Value ) ),

            FailureResult<TValue, TReason> f
                => Result<TOut, TReason>.Failure( f.Reason, f.Error ),

            _ => throw new InvalidOperationException()
        };
    }

    public Result<TValue, TOutReason> MapError<TOutReason>( Func<TReason, TOutReason> failure )
    {
        return this switch
        {
            SuccessResult<TValue, TReason> s
                => Result<TValue, TOutReason>.Success( s.Value ),

            FailureResult<TValue, TReason> f
                => Result<TValue, TOutReason>.Failure( failure( f.Reason ), f.Error ),

            _ => throw new InvalidOperationException()
        };
    }

    public Result<TOut, TReason> Bind<TOut>(
        Func<TValue, Result<TOut, TReason>> func )
    {
        return this switch
        {
            SuccessResult<TValue, TReason> s
                => func( s.Value ),

            FailureResult<TValue, TReason> f
                => Result<TOut, TReason>.Failure( f.Reason, f.Error ),

            _ => throw new InvalidOperationException()
        };
    }

    public Result<TValue, TReason> OnSuccess( Action<TValue> func )
    {
        if( this is SuccessResult<TValue, TReason> s )
        {
            func( s.Value );
        }

        return this;
    }

    public Result<TValue, TReason> OnFailure( Action<TReason, Exception?> func )
    {
        if( this is FailureResult<TValue, TReason> f )
        {
            func( f.Reason, f.Error );
        }

        return this;
    }

    public TResult Match<TResult>(
        Func<TValue, TResult> success,
        Func<TReason, Exception?, TResult> failure )
    {
        return this switch
        {
            SuccessResult<TValue, TReason> s => success( s.Value ),
            FailureResult<TValue, TReason> f => failure( f.Reason, f.Error ),
            _                                => throw new InvalidOperationException()
        };
    }

    public TValue Unwrap()
    {
        return this switch
        {
            SuccessResult<TValue, TReason> s => s.Value,
            FailureResult<TValue, TReason> f => throw new ResultException<TReason>(
                f.Reason,
                $"Result was a failure: {f.Reason}",
                f.Error
            ),
            _ => throw new InvalidOperationException()
        };
    }

    public TValue UnwrapOr( TValue defaultValue )
    {
        return this switch
        {
            SuccessResult<TValue, TReason> s => s.Value,
            FailureResult<TValue, TReason>   => defaultValue,
            _                                => throw new InvalidOperationException()
        };
    }

    public(TReason Reason, Exception? Error) UnwrapError()
    {
        return this switch
        {
            FailureResult<TValue, TReason> f => ( f.Reason, f.Error ),
            SuccessResult<TValue, TReason> s => throw new ResultException<TReason>(
                default,
                $"Result was a success: {s.Value}"
            ),
            _ => throw new InvalidOperationException()
        };
    }
}

public sealed class ResultException<TReason>(
    TReason? reason,
    string? message = null,
    Exception? innerException = null
) : Exception( message, innerException )
{
    // ReSharper disable once MemberCanBePrivate.Global
    public TReason? Reason { get; } = reason;

    public override string ToString()
        => $"ResultException: {Reason}";
}

internal sealed class SuccessResult<TValue, TReason>(
    TValue value
) : Result<TValue, TReason>
{
    public TValue Value { get; } = value;

    public override string ToString()
        => $"Success: {Value}";
}

internal sealed class FailureResult<TValue, TReason>(
    TReason reason,
    Exception? error = null
) : Result<TValue, TReason>
{
    public new TReason Reason { get; } = reason;
    public Exception? Error { get; } = error;

    public override string ToString()
        => $"Failure: {Reason}";
}
