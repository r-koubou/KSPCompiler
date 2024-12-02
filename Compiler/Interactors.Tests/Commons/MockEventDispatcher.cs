using System;
using System.Collections.Generic;

using KSPCompiler.Domain.Events;

namespace KSPCompiler.Interactors.Tests.Commons;

public sealed class MockEventDispatcher : IEventDispatcher
{
    public void Dispose() {}

    private List<IObserver<CompilationFatalEvent>> fatals = [];
    private List<IObserver<CompilationErrorEvent>> errors = [];
    private List<IObserver<CompilationWarningEvent>> warnings = [];

    public IDisposable Subscribe<TEvent>( IObserver<TEvent> observer ) where TEvent : IEvent
    {
        if( observer is IObserver<CompilationFatalEvent> fatal )
        {
            fatals.Add( fatal );
        }
        else if( observer is IObserver<CompilationErrorEvent> error )
        {
            errors.Add( error );
        }
        else if( observer is IObserver<CompilationWarningEvent> warning )
        {
            warnings.Add( warning );
        }
        else
        {
            throw new NotSupportedException( typeof( TEvent ).Name );
        }

        return new MockDisposable<TEvent>( this, observer );
    }

    public void Dispatch<TEvent>( TEvent evt ) where TEvent : IEvent
    {
        switch( evt )
        {
            case CompilationFatalEvent fatal:
                foreach( var observer in fatals )
                {
                    observer.OnNext( fatal );
                }
                break;

            case CompilationErrorEvent error:
                foreach( var observer in errors )
                {
                    observer.OnNext( error );
                }
                break;

            case CompilationWarningEvent warning:
                foreach( var observer in warnings )
                {
                    observer.OnNext( warning );
                }
                break;

            default:
                throw new NotSupportedException( evt.GetType().Name );
        }
    }

    public IObservable<TEvent> AsObservable<TEvent>() where TEvent : IEvent
        => throw new NotSupportedException();

    private class MockDisposable<TEvent>( MockEventDispatcher owner, IObserver<TEvent> observer ) : IDisposable
    {
        private readonly MockEventDispatcher owner = owner;
        private readonly IObserver<TEvent> observer = observer;

        public void Dispose()
        {
            switch( observer )
            {
                case IObserver<CompilationFatalEvent> fatal:
                    owner.fatals.Remove( fatal );
                    break;

                case IObserver<CompilationErrorEvent> error:
                    owner.errors.Remove( error );
                    break;

                case IObserver<CompilationWarningEvent> warning:
                    owner.warnings.Remove( warning );
                    break;
            }
        }
    }
}
