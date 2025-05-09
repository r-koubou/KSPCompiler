/* =========================================================================

    KSPLexer.g4
    Copyright (c) R-Koubou

   ======================================================================== */

lexer grammar KSPLexer;

channels {
    COMMENT_CHANNEL
}

//------------------------------------------------------------------------------
// 改行
// パース時に改行が必要なのでスキップしない
//------------------------------------------------------------------------------
EOL
    : CR LF? //(CR LF?)
    | LF
    //| MULTI_LINE_DELIMITER
;

fragment CR : '\r';
fragment LF : '\n';

MULTI_LINE_DELIMITER
    : '...' [ \t]* EOL
;

//------------------------------------------------------------------------------
// Skip: 空白 / コメント
//------------------------------------------------------------------------------

Whitespace
    : [ \t\f]+ -> channel(HIDDEN)
;

BlockComment
    : '{' .*? '}' -> channel(COMMENT_CHANNEL)
;

//------------------------------------------------------------------------------
// キーワード (KSP 標準)
//------------------------------------------------------------------------------

// Variable
DECLARE:                'declare';
// Callback
ON:                     'on';
END:                    'end';
FUNCTION:               'function';
// Statement
IF:                     'if';
ELSE:                   'else';
SELECT:                 'select';
CASE:                   'case';
TO:                     'to';
WHILE:                  'while';
CALL:                   'call';
// Control Statement
CONTINUE:               'continue';

//------------------------------------------------------------------------------
// プリプロセッサ (KSP 標準)
//------------------------------------------------------------------------------

PREPROCESSOR_SET_COND:      'SET_CONDITION';
PREPROCESSOR_RESET_COND:    'RESET_CONDITION';
PREPROCESSOR_CODE_IF:       'USE_CODE_IF';
PREPROCESSOR_CODE_IF_NOT:   'USE_CODE_IF_NOT';
PREPROCESSOR_CODE_END_IF:   'END_USE_CODE';

//------------------------------------------------------------------------------
// 演算子
//------------------------------------------------------------------------------

// 比較演算子
BOOL_GT:    '>';
BOOL_LT:    '<';
BOOL_GE:    '>=';
BOOL_LE:    '<=';
BOOL_EQ:    '=';
//TODO in_rangeはコマンドなので演算子扱いしない
BOOL_NE:    '#';
BOOL_NOT:   'not';
BOOL_AND:   'and';
BOOL_OR:    'or';
BOOL_XOR:   'xor';
// 算術演算
ASSIGN:     ':=';
PLUS:       '+';
MINUS:      '-';
MUL:        '*';
DIV:        '/';
MOD:        'mod';
BIT_AND:    '.and.';
BIT_OR:     '.or.';
BIT_NOT:    '.not.';
BIT_XOR:    '.xor.';
// ビットシフトはコマンドなので演算子扱いしない
// 文字列連結
STRING_ADD: '&';

//------------------------------------------------------------------------------
// 括弧、デリミタ
//------------------------------------------------------------------------------

LPARENT:    '(';
RPARENT:    ')';
LBRACKET:   '[';
RBRACKET:   ']';
COMMA:      ',';

//------------------------------------------------------------------------------
// リテラル
//------------------------------------------------------------------------------

INTEGER_LITERAL
    : DECIMAL_LITERAL
    | HEX_LITERAL
;

fragment DECIMAL_LITERAL
    : '0' | '1'..'9' '0'..'9'*
;

fragment HEX_LITERAL
    : '9' ('0'..'9' | 'a'..'f' | 'A'..'F')+ ('h' | 'H')
;

REAL_LITERAL
    : ( '0'..'9' )+ '.' ( '0'..'9' )+
;

STRING_LITERAL:
    '"'
        (~["\\\r\n] | EscapeSequence)*
    '"'
;

fragment EscapeSequence
    : '\\' [btnfr"'\\]
;


//------------------------------------------------------------------------------
// 識別子
//------------------------------------------------------------------------------

// 一般的なプログラミング言語と同じ、先頭文字に数字を指定することをさせない場合
// IDENTIFIER: VARIABLE_PREFIX? LETTER LETTER_OR_DIGIT*;

// KSPは変数名の先頭に数字を指定することができる
IDENTIFIER: VARIABLE_PREFIX? LETTER_OR_DIGIT+;

fragment LETTER
    : [a-zA-Z_]
;

fragment LETTER_OR_DIGIT
    : LETTER
    | [0-9]
;

fragment VARIABLE_PREFIX
    : [$%~?@!]
;
