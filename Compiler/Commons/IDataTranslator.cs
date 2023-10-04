namespace KSPCompiler.Commons;

public interface IDataTranslator<in TSource, out TTarget>
{
    TTarget Translate( TSource source );
}
