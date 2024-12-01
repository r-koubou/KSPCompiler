using System;

namespace KSPCompiler.Domain.Events;

/// <summary>
/// Empty implementation of <see cref="IEventDispatcher"/>.
/// </summary>
public class NullEventDispatcher : IEventDispatcher
{
    public static readonly NullEventDispatcher Instance = new();

    private NullEventDispatcher() {}

    public IDisposable Subscribe<TEvent>( IObserver<TEvent> observer ) where TEvent : IEvent
    {
        return new NullDisposable();
    }

    public void Dispatch<TEvent>( TEvent evt ) where TEvent : IEvent {}

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
