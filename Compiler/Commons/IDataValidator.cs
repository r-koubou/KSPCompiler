namespace KSPCompiler.Commons;

/// <summary>
/// Represents a validation rule for a data.
/// </summary>
public interface IDataValidator<in T>
{
    /// <summary>
    /// Validate the data.
    /// </summary>
    /// <param name="data">The data to validate.</param>
    /// <returns>True if the data is valid, otherwise false.</returns>
    bool Validate( T data );
}
