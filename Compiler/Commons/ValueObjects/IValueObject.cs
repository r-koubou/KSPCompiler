using System;

namespace KSPCompiler.Commons.ValueObjects;

public interface IValueObject<TValue> : IEquatable<TValue> where TValue : notnull
{
    TValue Value { get; init; }
}
