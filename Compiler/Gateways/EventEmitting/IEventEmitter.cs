using System;

namespace KSPCompiler.Gateways.EventEmitting;

/// <summary>
/// The basic interface for managing event subscribers and building event transmission management and execution.
/// </summary>
public interface IEventEmitter : IDisposable
{
    IDisposable Subscribe<TEvent>( IObserver<TEvent> observer ) where TEvent : IEvent;
    void Emit<TEvent>( TEvent evt ) where TEvent : IEvent;
    IObservable<TEvent> AsObservable<TEvent>() where TEvent : IEvent;
}
