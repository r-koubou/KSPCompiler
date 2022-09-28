namespace KSPCompiler.Commons.ValueObjects;

public abstract record ValueObject<TValue>( TValue Value ) : IValueObject<TValue> where TValue : notnull
{
    public sealed override string ToString()
        => ToStringImpl();

    // ReSharper disable once VirtualMemberNeverOverridden.Global
    protected virtual string ToStringImpl()
        => Value.ToString();
}
