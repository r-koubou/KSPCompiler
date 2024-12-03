using System;

namespace KSPCompiler.Domain.Events;

/// <summary>
/// Empty implementation of <see cref="IEventEmitter"/>.
/// </summary>
public class NullIEventEmitter : IEventEmitter
{
    public static readonly NullIEventEmitter Instance = new();

    private NullIEventEmitter() {}

    public IDisposable Subscribe<TEvent>( IObserver<TEvent> observer ) where TEvent : IEvent
    {
        return new NullDisposable();
    }

    public void Emit<TEvent>( TEvent evt ) where TEvent : IEvent {}

    public IObservable<TEvent> AsObservable<TEvent>() where TEvent : IEvent
    {
        return NullObservable<TEvent>.Instance;
    }

    public void Dispose() {}

    private class NullObservable<TEvent> : IObservable<TEvent> where TEvent : IEvent
    {
        public static readonly NullObservable<TEvent> Instance = new();

        public IDisposable Subscribe( IObserver<TEvent> observer )
        {
            return new NullDisposable();
        }
    }

    private class NullDisposable : IDisposable
    {
        public void Dispose() {}
    }
}
