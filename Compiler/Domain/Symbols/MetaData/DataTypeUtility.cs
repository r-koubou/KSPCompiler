using System;

namespace KSPCompiler.Domain.Symbols.MetaData;

public static class DataTypeUtility
{
    public static DataTypeFlag FromVariableName( string variableName )
    {
        if( string.IsNullOrEmpty( variableName ) )
        {
            return DataTypeFlag.None;
        }

        var typePrefix = variableName[ 0 ];

        if( KspRegExpConstants.NonTypePrefix.IsMatch( variableName ) )
        {
            return DataTypeFlag.TypeKspPreprocessorSymbol | DataTypeFlag.TypePgsId;
        }

        return typePrefix switch
        {
            //--------------------------------------------------------------------------
            // KSP Standard syntax
            //--------------------------------------------------------------------------
            '$' => DataTypeFlag.TypeInt,
            '%' => DataTypeFlag.TypeIntArray,
            '~' => DataTypeFlag.TypeReal,
            '?' => DataTypeFlag.TypeRealArray,
            '@' => DataTypeFlag.TypeString,
            '!' => DataTypeFlag.TypeStringArray,
            //--------------------------------------------------------------------------
            // For internal processing
            //--------------------------------------------------------------------------
            'B' => DataTypeFlag.TypeBool,
            'V' => DataTypeFlag.TypeVoid,
            'P' => DataTypeFlag.TypeKspPreprocessorSymbol,
            'K' => DataTypeFlag.TypePgsId,
            '*' => DataTypeFlag.MultipleType,
            _   => throw new ArgumentException( $"unknown ksp type : {typePrefix} ({nameof(variableName)}={variableName})" )
        };
    }
}
