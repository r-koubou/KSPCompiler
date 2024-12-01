using System;

namespace KSPCompiler.Domain.Events.Extensions;

public static class IEventDispatcherExtension
{
    public static IDisposable Subscribe<TEvent>( this IEventDispatcher self, Action<TEvent> action ) where TEvent : IEvent
    {
        return self.Subscribe( new AnonymousObserver<TEvent>( action ) );
    }

    private class AnonymousObserver<T> : IObserver<T> where T : IEvent
    {
        private readonly Action<T> onNext;
        private readonly Action? onCompleted;
        private readonly Action<Exception>? onError;

        public AnonymousObserver( Action<T> onNext, Action? onCompleted = null, Action<Exception>? onError = null )
        {
            this.onNext      = onNext;
            this.onCompleted = onCompleted;
            this.onError     = onError;
        }

        public void OnNext( T value )
            => onNext.Invoke( value );

        public void OnCompleted()
            => onCompleted?.Invoke();

        public void OnError( Exception error )
            => onError?.Invoke( error );
    }
}
