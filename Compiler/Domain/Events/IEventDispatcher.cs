using System;

namespace KSPCompiler.Domain.Events;

/// <summary>
/// The basic interface for managing event subscribers and building event transmission management and execution.
/// </summary>
public interface IEventDispatcher : IDisposable
{
    IDisposable Subscribe<TEvent>( IObserver<TEvent> observer ) where TEvent : IEvent;
    void Dispatch<TEvent>( TEvent evt ) where TEvent : IEvent;
    IObservable<TEvent> AsObservable<TEvent>() where TEvent : IEvent;
}
