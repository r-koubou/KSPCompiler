using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;

namespace KSPCompiler.Domain.Ast.Analyzers.Semantics;

public static class AssigningTypeUtility
{
    /// <summary>
    /// Judge whether the type of the right side can be assigned to the left side.
    /// </summary>
    /// <returns>true if the type of the right side can be assigned to the left side; otherwise, false.</returns>
    public static bool IsTypeCompatible( DataTypeFlag leftType, DataTypeFlag rightType )
    {
        // 暗黙の型変換
        // コマンドコールの戻り値が複数の型を持つなどで暗黙の型変換を要する場合
        // 代入先の変数に合わせる。暗黙の型変換が不可能な場合は、以降の型判定で失敗させる
        if( ( leftType & rightType ) != 0 )
        {
            // 型情報のマージ
            rightType |= leftType.TypeMasked(); // Mask: 型以外の属性は含まない
        }

        // 完全一致
        if( leftType == rightType )
        {
            return true;
        }

        // 個別に型チェック
        return IsTypeMatched( leftType, rightType );
    }

    private static bool IsTypeMatched( DataTypeFlag leftType, DataTypeFlag rightType )
    {
        var result = true;

        // 代入先：配列型
        // 代入元：非配列型
        if( leftType.IsArray() && !rightType.IsArray() )
        {
            result = false;
        }
        // 代入先：非配列型
        // 代入元：配列型
        else if( rightType.IsArray() && !leftType.IsArray() )
        {
            result = false;
        }
        // 型が一致するビットがない
        else if( ( leftType.TypeMasked() & rightType.TypeMasked() ) == 0 )
        {
            // 文字列型へ integer, real 値の代入は可能（文字列への型変換が発生）
            // -> 代入先が文字列型以外なら型の不一致 or 条件式は代入不可
            if( !leftType.IsString() || rightType.IsBoolean() )
            {
                result = false;
            }
        }

        return result;
    }
}
