using System;
using System.Linq;

namespace KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;

public static class DataTypeFlagUtility
{
    // ReSharper disable once MemberCanBePrivate.Global
    public static bool All( Func<DataTypeFlag, bool> eval, params DataTypeFlag[] flags )
        => flags.All( eval );

    // ReSharper disable once MemberCanBePrivate.Global
    public static bool Any( Func<DataTypeFlag, bool> eval, params DataTypeFlag[] flags )
        => flags.Any( eval );

    public static bool AllFallback( params DataTypeFlag[] flags )
    {
        return All( f => f == DataTypeFlag.FallBack, flags );
    }

    public static bool AnyFallback( params DataTypeFlag[] flags )
    {
        return Any( f => f == DataTypeFlag.FallBack, flags );
    }
}
