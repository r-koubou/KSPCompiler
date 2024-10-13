namespace KSPCompiler.Domain.Symbols;

public class CallbackSymbolTable : SymbolTable<CallbackSymbol>, ICallbackSymbolTable
{
    public override bool Add( CallbackSymbol symbol, bool overwrite = false )
    {
        var contains = table.ContainsKey( symbol.Name );

        // 既に登録済みの場合
        if( contains && !overwrite)
        {
            // 多重定義を許可されていないコールバックの場合は追加失敗扱い
            if( !symbol.AllowMultipleDeclaration )
            {
                return false;
            }
            // そうでない場合は追加済みとして扱う
            OnWillAdd( symbol );
            return true;
        }

        // 新規登録
        OnWillAdd( symbol );
        symbol.TableIndex    = uniqueIndexGenerator.Next();
        table[ symbol.Name ] = symbol;
        return true;
    }
}
