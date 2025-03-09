namespace KSPCompiler.Shared;

public interface IDataTranslator<in TSource, out TTarget>
{
    TTarget Translate( TSource source );
}
