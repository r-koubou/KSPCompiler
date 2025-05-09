using System;

namespace KSPCompiler.Shared.EventEmitting.Extensions;

public static class IDisposableExtension
{
    public static void AddTo( this IDisposable self, CompositeDisposable compositeDisposable )
    {
        compositeDisposable.Add( self );
    }
}
