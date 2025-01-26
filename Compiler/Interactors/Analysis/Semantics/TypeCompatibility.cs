using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;

namespace KSPCompiler.Interactors.Analysis.Semantics;

public static class TypeCompatibility
{
    /// <summary>
    /// Judge whether the type of the right side can be assigned to the left side. (=included implicit type conversion)
    /// </summary>
    /// <returns>true if the type of the right side can be assigned to the left side; otherwise, false.</returns>
    public static bool IsAssigningTypeCompatible( DataTypeFlag leftType, DataTypeFlag rightType )
    {
        // フォールバック・代替などの場合は型の判定を行わない
        if( DataTypeFlagUtility.AnyFallback( leftType, rightType ) )
        {
            return true;
        }

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
        return IsAssigningTypeMatched( leftType, rightType );
    }

    /// <summary>
    /// Check whether the type of the right side can be assigned to the left side. (=included implicit type conversion)
    /// </summary>
    private static bool IsAssigningTypeMatched( DataTypeFlag leftType, DataTypeFlag rightType )
    {
        var result = true;

        // フォールバック・代替などの場合は型の判定を行わない
        if( DataTypeFlagUtility.AnyFallback( leftType, rightType ) )
        {
            return true;
        }

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

    /// <summary>
    /// Judge given types are compatible. (=not included implicit type conversion)
    /// </summary>
    public static bool IsTypeCompatible( DataTypeFlag a, DataTypeFlag b )
    {
        // フォールバック・代替などの場合は型の判定を行わない
        if( DataTypeFlagUtility.AnyFallback( a, b ) )
        {
            return true;
        }

        // どちらかが全ての型を許容する場合は型の判定を行わない
        if( a == DataTypeFlag.All || b == DataTypeFlag.All )
        {
            return true;
        }

        // 完全一致
        if( a == b )
        {
            return true;
        }

        return IsTypeMatched( a, b );

    }

    /// <summary>
    /// Check given types are matched. (=not included implicit type conversion)
    /// </summary>
    private static bool IsTypeMatched( DataTypeFlag a, DataTypeFlag b )
    {
        var result = true;

        // フォールバック・代替などの場合は型の判定を行わない
        if( DataTypeFlagUtility.AnyFallback( a, b ) )
        {
            return true;
        }

        // a：配列型
        // b：非配列型
        if( a.IsArray() && !b.IsArray() )
        {
            result = false;
        }
        // a：非配列型
        // b：配列型
        else if( b.IsArray() && !a.IsArray() )
        {
            result = false;
        }
        // 型が一致するビットがない
        else if( ( a.TypeMasked() & b.TypeMasked() ) == 0 )
        {
            result = false;
        }

        return result;
    }
}
