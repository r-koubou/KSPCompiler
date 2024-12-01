using System;

using KSPCompiler.Commons;

namespace KSPCompiler.Domain.Events.Extensions;

public static class IDisposableExtension
{
    public static void AddTo( this IDisposable self, CompositeDisposable compositeDisposable )
    {
        compositeDisposable.Add( self );
    }
}
