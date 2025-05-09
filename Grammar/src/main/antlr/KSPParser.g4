/* =========================================================================

    KSPParser.g4
    Copyright (c) R-Koubou

   ======================================================================== */

parser grammar KSPParser;

options {
    tokenVocab = KSPLexer;
}

//------------------------------------------------------------------------------
// ノードのルート
//------------------------------------------------------------------------------
compilationUnit:
(
      callbackDeclaration
    | userFunctionDeclaration
    | EOL
    | MULTI_LINE_DELIMITER
)* EOF
;

//------------------------------------------------------------------------------
// 宣言時に指定可能な修飾子
//------------------------------------------------------------------------------
declarationModifier:
    IDENTIFIER
;

//------------------------------------------------------------------------------
// コールバック定義
//------------------------------------------------------------------------------
callbackDeclaration:
    ON
        MULTI_LINE_DELIMITER* name = IDENTIFIER
        MULTI_LINE_DELIMITER*
        (
            LPARENT
                MULTI_LINE_DELIMITER* arguments = argumentDefinitionList MULTI_LINE_DELIMITER*
            RPARENT
        )?
    EOL
    block
    END MULTI_LINE_DELIMITER* ON
;

//
// 引数宣言
//
argumentDefinitionList
    :   IDENTIFIER
    |   argumentDefinitionList COMMA IDENTIFIER
;

//------------------------------------------------------------------------------
// KSPユーザー定義関数定義
//------------------------------------------------------------------------------

userFunctionDeclaration:
    FUNCTION MULTI_LINE_DELIMITER* name = IDENTIFIER (LPARENT RPARENT)?
    EOL
    block
    END MULTI_LINE_DELIMITER* FUNCTION
;

//------------------------------------------------------------------------------
// コードブロック
//------------------------------------------------------------------------------
block
    : statement*
;

//------------------------------------------------------------------------------
// 変数宣言
//------------------------------------------------------------------------------
variableDeclaration:
    DECLARE MULTI_LINE_DELIMITER*
    modifier = declarationModifier*

    MULTI_LINE_DELIMITER*

    name = IDENTIFIER variableInitializer?
;

//
// 初期値代入
//
variableInitializer
    : arrayInitializer
    | primitiveInitializer
;

//
// 変数の初期値代入
//
primitiveInitializer
    : MULTI_LINE_DELIMITER* ASSIGN expression
    | uiInitializer
;

//
// 配列変数初期値代入
//
arrayInitializer:
    // [ 要素数 ]
    MULTI_LINE_DELIMITER*
    LBRACKET
    MULTI_LINE_DELIMITER*
        expression
    MULTI_LINE_DELIMITER*
    RBRACKET
    // 要素の初期値指定
    (
        // :=
        MULTI_LINE_DELIMITER*
        // UI変数の場合は := は不要、通常の配列変数の場合は必要
        // 文保解析では有無の正確性は許容し、意味解析で判定する
        assign = ASSIGN?
        //( x, x, x, ...)
        MULTI_LINE_DELIMITER*
        LPARENT
            expressionList
        RPARENT
    )?
;

//
// UI変数の初期化パラメーター指定
//
uiInitializer:
    MULTI_LINE_DELIMITER*
    LPARENT
        expressionList
    RPARENT
;


//------------------------------------------------------------------------------
// ステートメント
//------------------------------------------------------------------------------
statement
    : MULTI_LINE_DELIMITER
    | EOL
    | variableDeclaration
    | preprocessor
    | ifStatement
    | selectStatement
    | whileStatement
    | continueStatement
    | callUserFunction
    | expressionStatement
;

//------------------------------------------------------------------------------
// KSPプリプロセッサ
//------------------------------------------------------------------------------
preprocessor
    : preprocessorDefine
    | preprocessorUndefine
    | preprocessorIfdefine
    | preprocessorIfnotDefine
;

//
// define
//
preprocessorDefine:
    PREPROCESSOR_SET_COND MULTI_LINE_DELIMITER*             // SET_CONDITION
    LPARENT                                                 // (
    MULTI_LINE_DELIMITER*
    symbol = IDENTIFIER                                     // SYMBOL
    MULTI_LINE_DELIMITER*
    RPARENT                                                 // )
;

//
// undef
//
preprocessorUndefine:
    PREPROCESSOR_RESET_COND MULTI_LINE_DELIMITER*         // RESET_CONDITION
    LPARENT                                               // (
    MULTI_LINE_DELIMITER*
    symbol = IDENTIFIER                                   // SYMBOL
    MULTI_LINE_DELIMITER*
    RPARENT                                               // )
;

//
// ifdef
//
preprocessorIfdefine:
    PREPROCESSOR_CODE_IF MULTI_LINE_DELIMITER*            // USE_CODE_IF
    LPARENT                                               // (
    MULTI_LINE_DELIMITER*
    symbol = IDENTIFIER                                   // SYMBOL
    MULTI_LINE_DELIMITER*
    RPARENT                                               // )
    block                                                 // code
    PREPROCESSOR_CODE_END_IF                              // END_USE_CODE
;

//
// ifndef
//
preprocessorIfnotDefine:
    PREPROCESSOR_CODE_IF_NOT MULTI_LINE_DELIMITER*        // USE_CODE_IF_NOT
    LPARENT                                               // (
    MULTI_LINE_DELIMITER*
    symbol = IDENTIFIER                                   // SYMBOL
    MULTI_LINE_DELIMITER*
    RPARENT                                               // )
    block                                                 // code
    PREPROCESSOR_CODE_END_IF                              // END_USE_CODE
;

//------------------------------------------------------------------------------
// ifステートメント
//------------------------------------------------------------------------------
ifStatement:
    IF         MULTI_LINE_DELIMITER*        // if
    LPARENT                                 // (
    expression                              // cond
    RPARENT                                 // )
    ifBlock = block
    (
        ELSE
        elseBlock = block
    )?
    END MULTI_LINE_DELIMITER* IF
;

//------------------------------------------------------------------------------
// select〜case ステートメント
//------------------------------------------------------------------------------
selectStatement:
    SELECT   MULTI_LINE_DELIMITER*          // select
    LPARENT                                 // (
    expression MULTI_LINE_DELIMITER*        // cond
    RPARENT   MULTI_LINE_DELIMITER*         // )
    EOL+
    caseBlock+                          // case / case xxx to yyy
    END MULTI_LINE_DELIMITER* SELECT        // end select
;

//
// case
//
caseBlock:
    CASE MULTI_LINE_DELIMITER*                          // case
    condFrom = expression                               // cond
    (
        TO MULTI_LINE_DELIMITER* condTo = expression    // condFrom to condTo
    )?
    block
    MULTI_LINE_DELIMITER*
;

//------------------------------------------------------------------------------
// while ステートメント
//------------------------------------------------------------------------------

whileStatement:
    WHILE      MULTI_LINE_DELIMITER*        // while
        LPARENT                             // (
        expression                          // cond
        RPARENT                             // )
        block                               // statement+
    END MULTI_LINE_DELIMITER* WHILE
;

//------------------------------------------------------------------------------
// continue ステートメント
//------------------------------------------------------------------------------

continueStatement:
    CONTINUE MULTI_LINE_DELIMITER*          // continue
;

//------------------------------------------------------------------------------
// KSP ユーザー定義関数の呼び出し
//------------------------------------------------------------------------------

callUserFunction:
    CALL MULTI_LINE_DELIMITER* name = IDENTIFIER
;

//------------------------------------------------------------------------------
// 式ステートメント
//------------------------------------------------------------------------------
expressionStatement
    //------------------------------------------------------------------------------
    // 代入
    //------------------------------------------------------------------------------
    : assignmentExpression
    //------------------------------------------------------------------------------
    // KSPコマンド呼び出し
    //------------------------------------------------------------------------------
    |
        callExpr = primaryExpression
        (
            LPARENT
                // argumentExpressionList? MULTI_LINE_DELIMITER* // TODO: コマンド・関数の引数に代入式をサポートする場合に使用
                callArgs = expressionList? MULTI_LINE_DELIMITER*
            RPARENT
        )?
;

//------------------------------------------------------------------------------
// 式
//------------------------------------------------------------------------------

primaryExpression
    //------------------------------------------------------------------------------
    // ...
    //------------------------------------------------------------------------------
    : MULTI_LINE_DELIMITER primaryExpression
    //------------------------------------------------------------------------------
    // シンボル, KSP変数
    //------------------------------------------------------------------------------
    | IDENTIFIER
    //------------------------------------------------------------------------------
    // リテラル値
    //------------------------------------------------------------------------------
    | ( INTEGER_LITERAL | REAL_LITERAL | STRING_LITERAL )
    //------------------------------------------------------------------------------
    // カッコありの式
    //------------------------------------------------------------------------------
    | LPARENT expression RPARENT
;

//
// 後置式
//
postfixExpression
    //------------------------------------------------------------------------------
    // 式
    //------------------------------------------------------------------------------
    :  expr = primaryExpression MULTI_LINE_DELIMITER*
    //------------------------------------------------------------------------------
    // KSPコマンド呼び出し
    //------------------------------------------------------------------------------
    |
        callExpr = postfixExpression MULTI_LINE_DELIMITER*
        LPARENT
            // argumentExpressionList? MULTI_LINE_DELIMITER* // TODO: コマンド・関数の引数に代入式をサポートする場合に使用
            callArgs = expressionList? MULTI_LINE_DELIMITER*
        RPARENT
    //------------------------------------------------------------------------------
    // 配列要素参照
    //------------------------------------------------------------------------------
    |
        arrayExpr = postfixExpression MULTI_LINE_DELIMITER*
        LBRACKET
            arrayIndexExpr = expression MULTI_LINE_DELIMITER*
        RBRACKET
;

//
// 代入式
//
assignmentExpression
    : postfixExpression assignmentOperator expression
;

//
// カンマ区切りで記述可能な式のリスト（代入式も含む）
// KSP非標準
//
assignmentExpressionList
    : assignmentExpression
    | assignmentExpressionList COMMA assignmentExpression
;

//
// 代入演算子
//
assignmentOperator
    : opr = ASSIGN
    // +=, -= などをやるならここ
;


//
// 式
//
expression
    : stringConcatenateExpression
;

//
// カンマ区切りで記述可能な式のリスト（代入式は含まない）
//
expressionList
    : expression
    | expressionList COMMA expression
;

//
// 連結(->連結後、文字列へ暗黙の型変換)
//
stringConcatenateExpression
    : nested = logicalOrExpression
    | left   = stringConcatenateExpression STRING_ADD right = logicalOrExpression
;

//
// 条件式OR
//
logicalOrExpression
    : nested = logicalAndExpression
    | left   = logicalOrExpression BOOL_OR right = logicalAndExpression
;

//
// 条件式AND
//
logicalAndExpression
    : nested = logicalXorExpression
    | left   = logicalAndExpression BOOL_AND right = logicalXorExpression
;

//
// 条件式XOR
//
logicalXorExpression
    : nested = bitwiseOrExpression
    | left   = logicalXorExpression BOOL_XOR right = bitwiseOrExpression
;

//
// 論理積
//
bitwiseOrExpression
    : nested = bitwiseAndExpression
    | left   = bitwiseOrExpression BIT_OR right = bitwiseAndExpression
;

//
// 論理和
//
bitwiseAndExpression
    : nested = bitwiseXorExpression
    | left   = bitwiseAndExpression BIT_AND right = bitwiseXorExpression
;

//
// 排他的論理和
//
bitwiseXorExpression
    : nested = equalityExpression
    | left   = bitwiseXorExpression BIT_XOR right = equalityExpression
;

//
// 比較 (一致, 不一致)
//
equalityExpression
    : nested = relationalExpression
    | left   = equalityExpression opr = BOOL_EQ right = relationalExpression
    | left   = equalityExpression opr = BOOL_NE right = relationalExpression
;

//
// 比較（不等号）
//
relationalExpression
    : nested = additiveExpression
    | left   = relationalExpression opr = BOOL_LT right = additiveExpression
    | left   = relationalExpression opr = BOOL_GT right = additiveExpression
    | left   = relationalExpression opr = BOOL_LE right = additiveExpression
    | left   = relationalExpression opr = BOOL_GE right = additiveExpression
;

//
// 加算
//
additiveExpression
    : nested = multiplicativeExpression
    | left   = additiveExpression opr = PLUS  right = multiplicativeExpression
    | left   = additiveExpression opr = MINUS right = multiplicativeExpression
;

//
// 乗算
//
multiplicativeExpression
    : nested = unaryExpression
    | left   = multiplicativeExpression opr = MUL right = unaryExpression
    | left   = multiplicativeExpression opr = DIV right = unaryExpression
    | left   = multiplicativeExpression opr = MOD right = unaryExpression
;

//
// 単項
//
unaryExpression
    : nested = postfixExpression
    | opr    = MINUS   unaryMinus  = unaryExpression
    | opr    = BIT_NOT unaryNot    = unaryExpression
    | opr    = BOOL_NOT logicalNot = unaryExpression
;

//
// 引数リスト
// TODO: コマンド・関数の引数に代入式をサポートする場合に使用
// argumentExpressionList
//     : assignmentExpression
//     | argumentExpressionList COMMA assignmentExpression
// ;
