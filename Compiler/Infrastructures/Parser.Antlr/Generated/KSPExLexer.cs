//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from KSPExLexer.g4 by ANTLR 4.13.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace KSPCompiler.Infrastructures.Parser.Antlr {
using System;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.1")]
[System.CLSCompliant(false)]
public partial class KSPExLexer : Lexer {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		INTEGER_LITERAL=1, EOL=2, MULTI_LINE_DELIMITER=3, Whitespace=4, BlockComment=5, 
		DECLARE=6, ON=7, END=8, FUNCTION=9, IF=10, ELSE=11, SELECT=12, CASE=13, 
		TO=14, WHILE=15, CALL=16, CONTINUE=17, PREPROCESSOR_SET_COND=18, PREPROCESSOR_RESET_COND=19, 
		PREPROCESSOR_CODE_IF=20, PREPROCESSOR_CODE_IF_NOT=21, PREPROCESSOR_CODE_END_IF=22, 
		BOOL_GT=23, BOOL_LT=24, BOOL_GE=25, BOOL_LE=26, BOOL_EQ=27, BOOL_NE=28, 
		BOOL_NOT=29, BOOL_AND=30, BOOL_OR=31, BOOL_XOR=32, ASSIGN=33, PLUS=34, 
		MINUS=35, MUL=36, DIV=37, MOD=38, BIT_AND=39, BIT_OR=40, BIT_NOT=41, BIT_XOR=42, 
		STRING_ADD=43, LPARENT=44, RPARENT=45, LBRACKET=46, RBRACKET=47, COMMA=48, 
		REAL_LITERAL=49, STRING_LITERAL=50, IDENTIFIER=51;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"INTEGER_LITERAL", "DECIMAL_LITERAL", "HEX_LITERAL", "EXT_HEX_LITERAL", 
		"EXT_BIN_LITERAL", "EOL", "CR", "LF", "MULTI_LINE_DELIMITER", "Whitespace", 
		"BlockComment", "DECLARE", "ON", "END", "FUNCTION", "IF", "ELSE", "SELECT", 
		"CASE", "TO", "WHILE", "CALL", "CONTINUE", "PREPROCESSOR_SET_COND", "PREPROCESSOR_RESET_COND", 
		"PREPROCESSOR_CODE_IF", "PREPROCESSOR_CODE_IF_NOT", "PREPROCESSOR_CODE_END_IF", 
		"BOOL_GT", "BOOL_LT", "BOOL_GE", "BOOL_LE", "BOOL_EQ", "BOOL_NE", "BOOL_NOT", 
		"BOOL_AND", "BOOL_OR", "BOOL_XOR", "ASSIGN", "PLUS", "MINUS", "MUL", "DIV", 
		"MOD", "BIT_AND", "BIT_OR", "BIT_NOT", "BIT_XOR", "STRING_ADD", "LPARENT", 
		"RPARENT", "LBRACKET", "RBRACKET", "COMMA", "REAL_LITERAL", "STRING_LITERAL", 
		"EscapeSequence", "IDENTIFIER", "LETTER", "LETTER_OR_DIGIT", "VARIABLE_PREFIX"
	};


	public KSPExLexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public KSPExLexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, null, null, null, null, null, "'declare'", "'on'", "'end'", "'function'", 
		"'if'", "'else'", "'select'", "'case'", "'to'", "'while'", "'call'", "'continue'", 
		"'SET_CONDITION'", "'RESET_CONDITION'", "'USE_CODE_IF'", "'USE_CODE_IF_NOT'", 
		"'END_USE_CODE'", "'>'", "'<'", "'>='", "'<='", "'='", "'#'", "'not'", 
		"'and'", "'or'", "'xor'", "':='", "'+'", "'-'", "'*'", "'/'", "'mod'", 
		"'.and.'", "'.or.'", "'.not.'", "'.xor.'", "'&'", "'('", "')'", "'['", 
		"']'", "','"
	};
	private static readonly string[] _SymbolicNames = {
		null, "INTEGER_LITERAL", "EOL", "MULTI_LINE_DELIMITER", "Whitespace", 
		"BlockComment", "DECLARE", "ON", "END", "FUNCTION", "IF", "ELSE", "SELECT", 
		"CASE", "TO", "WHILE", "CALL", "CONTINUE", "PREPROCESSOR_SET_COND", "PREPROCESSOR_RESET_COND", 
		"PREPROCESSOR_CODE_IF", "PREPROCESSOR_CODE_IF_NOT", "PREPROCESSOR_CODE_END_IF", 
		"BOOL_GT", "BOOL_LT", "BOOL_GE", "BOOL_LE", "BOOL_EQ", "BOOL_NE", "BOOL_NOT", 
		"BOOL_AND", "BOOL_OR", "BOOL_XOR", "ASSIGN", "PLUS", "MINUS", "MUL", "DIV", 
		"MOD", "BIT_AND", "BIT_OR", "BIT_NOT", "BIT_XOR", "STRING_ADD", "LPARENT", 
		"RPARENT", "LBRACKET", "RBRACKET", "COMMA", "REAL_LITERAL", "STRING_LITERAL", 
		"IDENTIFIER"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "KSPExLexer.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ChannelNames { get { return channelNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override int[] SerializedAtn { get { return _serializedATN; } }

	static KSPExLexer() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}
	private static int[] _serializedATN = {
		4,0,51,458,6,-1,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,
		6,2,7,7,7,2,8,7,8,2,9,7,9,2,10,7,10,2,11,7,11,2,12,7,12,2,13,7,13,2,14,
		7,14,2,15,7,15,2,16,7,16,2,17,7,17,2,18,7,18,2,19,7,19,2,20,7,20,2,21,
		7,21,2,22,7,22,2,23,7,23,2,24,7,24,2,25,7,25,2,26,7,26,2,27,7,27,2,28,
		7,28,2,29,7,29,2,30,7,30,2,31,7,31,2,32,7,32,2,33,7,33,2,34,7,34,2,35,
		7,35,2,36,7,36,2,37,7,37,2,38,7,38,2,39,7,39,2,40,7,40,2,41,7,41,2,42,
		7,42,2,43,7,43,2,44,7,44,2,45,7,45,2,46,7,46,2,47,7,47,2,48,7,48,2,49,
		7,49,2,50,7,50,2,51,7,51,2,52,7,52,2,53,7,53,2,54,7,54,2,55,7,55,2,56,
		7,56,2,57,7,57,2,58,7,58,2,59,7,59,2,60,7,60,1,0,1,0,1,0,1,0,3,0,128,8,
		0,1,1,1,1,5,1,132,8,1,10,1,12,1,135,9,1,1,2,1,2,4,2,139,8,2,11,2,12,2,
		140,1,2,1,2,1,3,1,3,1,3,1,3,4,3,149,8,3,11,3,12,3,150,1,4,1,4,1,4,1,4,
		4,4,157,8,4,11,4,12,4,158,1,5,1,5,3,5,163,8,5,1,5,3,5,166,8,5,1,6,1,6,
		1,7,1,7,1,8,1,8,1,8,1,8,1,8,5,8,177,8,8,10,8,12,8,180,9,8,1,8,1,8,1,9,
		4,9,185,8,9,11,9,12,9,186,1,9,1,9,1,10,1,10,5,10,193,8,10,10,10,12,10,
		196,9,10,1,10,1,10,1,10,1,10,1,11,1,11,1,11,1,11,1,11,1,11,1,11,1,11,1,
		12,1,12,1,12,1,13,1,13,1,13,1,13,1,14,1,14,1,14,1,14,1,14,1,14,1,14,1,
		14,1,14,1,15,1,15,1,15,1,16,1,16,1,16,1,16,1,16,1,17,1,17,1,17,1,17,1,
		17,1,17,1,17,1,18,1,18,1,18,1,18,1,18,1,19,1,19,1,19,1,20,1,20,1,20,1,
		20,1,20,1,20,1,21,1,21,1,21,1,21,1,21,1,22,1,22,1,22,1,22,1,22,1,22,1,
		22,1,22,1,22,1,23,1,23,1,23,1,23,1,23,1,23,1,23,1,23,1,23,1,23,1,23,1,
		23,1,23,1,23,1,24,1,24,1,24,1,24,1,24,1,24,1,24,1,24,1,24,1,24,1,24,1,
		24,1,24,1,24,1,24,1,24,1,25,1,25,1,25,1,25,1,25,1,25,1,25,1,25,1,25,1,
		25,1,25,1,25,1,26,1,26,1,26,1,26,1,26,1,26,1,26,1,26,1,26,1,26,1,26,1,
		26,1,26,1,26,1,26,1,26,1,27,1,27,1,27,1,27,1,27,1,27,1,27,1,27,1,27,1,
		27,1,27,1,27,1,27,1,28,1,28,1,29,1,29,1,30,1,30,1,30,1,31,1,31,1,31,1,
		32,1,32,1,33,1,33,1,34,1,34,1,34,1,34,1,35,1,35,1,35,1,35,1,36,1,36,1,
		36,1,37,1,37,1,37,1,37,1,38,1,38,1,38,1,39,1,39,1,40,1,40,1,41,1,41,1,
		42,1,42,1,43,1,43,1,43,1,43,1,44,1,44,1,44,1,44,1,44,1,44,1,45,1,45,1,
		45,1,45,1,45,1,46,1,46,1,46,1,46,1,46,1,46,1,47,1,47,1,47,1,47,1,47,1,
		47,1,48,1,48,1,49,1,49,1,50,1,50,1,51,1,51,1,52,1,52,1,53,1,53,1,54,4,
		54,420,8,54,11,54,12,54,421,1,54,1,54,4,54,426,8,54,11,54,12,54,427,1,
		55,1,55,1,55,5,55,433,8,55,10,55,12,55,436,9,55,1,55,1,55,1,56,1,56,1,
		56,1,57,3,57,444,8,57,1,57,4,57,447,8,57,11,57,12,57,448,1,58,1,58,1,59,
		1,59,3,59,455,8,59,1,60,1,60,1,194,0,61,1,1,3,0,5,0,7,0,9,0,11,2,13,0,
		15,0,17,3,19,4,21,5,23,6,25,7,27,8,29,9,31,10,33,11,35,12,37,13,39,14,
		41,15,43,16,45,17,47,18,49,19,51,20,53,21,55,22,57,23,59,24,61,25,63,26,
		65,27,67,28,69,29,71,30,73,31,75,32,77,33,79,34,81,35,83,36,85,37,87,38,
		89,39,91,40,93,41,95,42,97,43,99,44,101,45,103,46,105,47,107,48,109,49,
		111,50,113,0,115,51,117,0,119,0,121,0,1,0,10,3,0,48,57,65,70,97,102,2,
		0,72,72,104,104,1,0,48,49,2,0,9,9,32,32,3,0,9,9,12,12,32,32,4,0,10,10,
		13,13,34,34,92,92,8,0,34,34,39,39,92,92,98,98,102,102,110,110,114,114,
		116,116,3,0,65,90,95,95,97,122,1,0,48,57,4,0,33,33,36,37,63,64,126,126,
		466,0,1,1,0,0,0,0,11,1,0,0,0,0,17,1,0,0,0,0,19,1,0,0,0,0,21,1,0,0,0,0,
		23,1,0,0,0,0,25,1,0,0,0,0,27,1,0,0,0,0,29,1,0,0,0,0,31,1,0,0,0,0,33,1,
		0,0,0,0,35,1,0,0,0,0,37,1,0,0,0,0,39,1,0,0,0,0,41,1,0,0,0,0,43,1,0,0,0,
		0,45,1,0,0,0,0,47,1,0,0,0,0,49,1,0,0,0,0,51,1,0,0,0,0,53,1,0,0,0,0,55,
		1,0,0,0,0,57,1,0,0,0,0,59,1,0,0,0,0,61,1,0,0,0,0,63,1,0,0,0,0,65,1,0,0,
		0,0,67,1,0,0,0,0,69,1,0,0,0,0,71,1,0,0,0,0,73,1,0,0,0,0,75,1,0,0,0,0,77,
		1,0,0,0,0,79,1,0,0,0,0,81,1,0,0,0,0,83,1,0,0,0,0,85,1,0,0,0,0,87,1,0,0,
		0,0,89,1,0,0,0,0,91,1,0,0,0,0,93,1,0,0,0,0,95,1,0,0,0,0,97,1,0,0,0,0,99,
		1,0,0,0,0,101,1,0,0,0,0,103,1,0,0,0,0,105,1,0,0,0,0,107,1,0,0,0,0,109,
		1,0,0,0,0,111,1,0,0,0,0,115,1,0,0,0,1,127,1,0,0,0,3,129,1,0,0,0,5,136,
		1,0,0,0,7,144,1,0,0,0,9,152,1,0,0,0,11,165,1,0,0,0,13,167,1,0,0,0,15,169,
		1,0,0,0,17,171,1,0,0,0,19,184,1,0,0,0,21,190,1,0,0,0,23,201,1,0,0,0,25,
		209,1,0,0,0,27,212,1,0,0,0,29,216,1,0,0,0,31,225,1,0,0,0,33,228,1,0,0,
		0,35,233,1,0,0,0,37,240,1,0,0,0,39,245,1,0,0,0,41,248,1,0,0,0,43,254,1,
		0,0,0,45,259,1,0,0,0,47,268,1,0,0,0,49,282,1,0,0,0,51,298,1,0,0,0,53,310,
		1,0,0,0,55,326,1,0,0,0,57,339,1,0,0,0,59,341,1,0,0,0,61,343,1,0,0,0,63,
		346,1,0,0,0,65,349,1,0,0,0,67,351,1,0,0,0,69,353,1,0,0,0,71,357,1,0,0,
		0,73,361,1,0,0,0,75,364,1,0,0,0,77,368,1,0,0,0,79,371,1,0,0,0,81,373,1,
		0,0,0,83,375,1,0,0,0,85,377,1,0,0,0,87,379,1,0,0,0,89,383,1,0,0,0,91,389,
		1,0,0,0,93,394,1,0,0,0,95,400,1,0,0,0,97,406,1,0,0,0,99,408,1,0,0,0,101,
		410,1,0,0,0,103,412,1,0,0,0,105,414,1,0,0,0,107,416,1,0,0,0,109,419,1,
		0,0,0,111,429,1,0,0,0,113,439,1,0,0,0,115,443,1,0,0,0,117,450,1,0,0,0,
		119,454,1,0,0,0,121,456,1,0,0,0,123,128,3,3,1,0,124,128,3,5,2,0,125,128,
		3,7,3,0,126,128,3,9,4,0,127,123,1,0,0,0,127,124,1,0,0,0,127,125,1,0,0,
		0,127,126,1,0,0,0,128,2,1,0,0,0,129,133,2,49,57,0,130,132,2,48,57,0,131,
		130,1,0,0,0,132,135,1,0,0,0,133,131,1,0,0,0,133,134,1,0,0,0,134,4,1,0,
		0,0,135,133,1,0,0,0,136,138,5,57,0,0,137,139,7,0,0,0,138,137,1,0,0,0,139,
		140,1,0,0,0,140,138,1,0,0,0,140,141,1,0,0,0,141,142,1,0,0,0,142,143,7,
		1,0,0,143,6,1,0,0,0,144,145,5,48,0,0,145,146,5,120,0,0,146,148,1,0,0,0,
		147,149,7,0,0,0,148,147,1,0,0,0,149,150,1,0,0,0,150,148,1,0,0,0,150,151,
		1,0,0,0,151,8,1,0,0,0,152,153,5,48,0,0,153,154,5,98,0,0,154,156,1,0,0,
		0,155,157,7,2,0,0,156,155,1,0,0,0,157,158,1,0,0,0,158,156,1,0,0,0,158,
		159,1,0,0,0,159,10,1,0,0,0,160,162,3,13,6,0,161,163,3,15,7,0,162,161,1,
		0,0,0,162,163,1,0,0,0,163,166,1,0,0,0,164,166,3,15,7,0,165,160,1,0,0,0,
		165,164,1,0,0,0,166,12,1,0,0,0,167,168,5,13,0,0,168,14,1,0,0,0,169,170,
		5,10,0,0,170,16,1,0,0,0,171,172,5,46,0,0,172,173,5,46,0,0,173,174,5,46,
		0,0,174,178,1,0,0,0,175,177,7,3,0,0,176,175,1,0,0,0,177,180,1,0,0,0,178,
		176,1,0,0,0,178,179,1,0,0,0,179,181,1,0,0,0,180,178,1,0,0,0,181,182,3,
		11,5,0,182,18,1,0,0,0,183,185,7,4,0,0,184,183,1,0,0,0,185,186,1,0,0,0,
		186,184,1,0,0,0,186,187,1,0,0,0,187,188,1,0,0,0,188,189,6,9,0,0,189,20,
		1,0,0,0,190,194,5,123,0,0,191,193,9,0,0,0,192,191,1,0,0,0,193,196,1,0,
		0,0,194,195,1,0,0,0,194,192,1,0,0,0,195,197,1,0,0,0,196,194,1,0,0,0,197,
		198,5,125,0,0,198,199,1,0,0,0,199,200,6,10,0,0,200,22,1,0,0,0,201,202,
		5,100,0,0,202,203,5,101,0,0,203,204,5,99,0,0,204,205,5,108,0,0,205,206,
		5,97,0,0,206,207,5,114,0,0,207,208,5,101,0,0,208,24,1,0,0,0,209,210,5,
		111,0,0,210,211,5,110,0,0,211,26,1,0,0,0,212,213,5,101,0,0,213,214,5,110,
		0,0,214,215,5,100,0,0,215,28,1,0,0,0,216,217,5,102,0,0,217,218,5,117,0,
		0,218,219,5,110,0,0,219,220,5,99,0,0,220,221,5,116,0,0,221,222,5,105,0,
		0,222,223,5,111,0,0,223,224,5,110,0,0,224,30,1,0,0,0,225,226,5,105,0,0,
		226,227,5,102,0,0,227,32,1,0,0,0,228,229,5,101,0,0,229,230,5,108,0,0,230,
		231,5,115,0,0,231,232,5,101,0,0,232,34,1,0,0,0,233,234,5,115,0,0,234,235,
		5,101,0,0,235,236,5,108,0,0,236,237,5,101,0,0,237,238,5,99,0,0,238,239,
		5,116,0,0,239,36,1,0,0,0,240,241,5,99,0,0,241,242,5,97,0,0,242,243,5,115,
		0,0,243,244,5,101,0,0,244,38,1,0,0,0,245,246,5,116,0,0,246,247,5,111,0,
		0,247,40,1,0,0,0,248,249,5,119,0,0,249,250,5,104,0,0,250,251,5,105,0,0,
		251,252,5,108,0,0,252,253,5,101,0,0,253,42,1,0,0,0,254,255,5,99,0,0,255,
		256,5,97,0,0,256,257,5,108,0,0,257,258,5,108,0,0,258,44,1,0,0,0,259,260,
		5,99,0,0,260,261,5,111,0,0,261,262,5,110,0,0,262,263,5,116,0,0,263,264,
		5,105,0,0,264,265,5,110,0,0,265,266,5,117,0,0,266,267,5,101,0,0,267,46,
		1,0,0,0,268,269,5,83,0,0,269,270,5,69,0,0,270,271,5,84,0,0,271,272,5,95,
		0,0,272,273,5,67,0,0,273,274,5,79,0,0,274,275,5,78,0,0,275,276,5,68,0,
		0,276,277,5,73,0,0,277,278,5,84,0,0,278,279,5,73,0,0,279,280,5,79,0,0,
		280,281,5,78,0,0,281,48,1,0,0,0,282,283,5,82,0,0,283,284,5,69,0,0,284,
		285,5,83,0,0,285,286,5,69,0,0,286,287,5,84,0,0,287,288,5,95,0,0,288,289,
		5,67,0,0,289,290,5,79,0,0,290,291,5,78,0,0,291,292,5,68,0,0,292,293,5,
		73,0,0,293,294,5,84,0,0,294,295,5,73,0,0,295,296,5,79,0,0,296,297,5,78,
		0,0,297,50,1,0,0,0,298,299,5,85,0,0,299,300,5,83,0,0,300,301,5,69,0,0,
		301,302,5,95,0,0,302,303,5,67,0,0,303,304,5,79,0,0,304,305,5,68,0,0,305,
		306,5,69,0,0,306,307,5,95,0,0,307,308,5,73,0,0,308,309,5,70,0,0,309,52,
		1,0,0,0,310,311,5,85,0,0,311,312,5,83,0,0,312,313,5,69,0,0,313,314,5,95,
		0,0,314,315,5,67,0,0,315,316,5,79,0,0,316,317,5,68,0,0,317,318,5,69,0,
		0,318,319,5,95,0,0,319,320,5,73,0,0,320,321,5,70,0,0,321,322,5,95,0,0,
		322,323,5,78,0,0,323,324,5,79,0,0,324,325,5,84,0,0,325,54,1,0,0,0,326,
		327,5,69,0,0,327,328,5,78,0,0,328,329,5,68,0,0,329,330,5,95,0,0,330,331,
		5,85,0,0,331,332,5,83,0,0,332,333,5,69,0,0,333,334,5,95,0,0,334,335,5,
		67,0,0,335,336,5,79,0,0,336,337,5,68,0,0,337,338,5,69,0,0,338,56,1,0,0,
		0,339,340,5,62,0,0,340,58,1,0,0,0,341,342,5,60,0,0,342,60,1,0,0,0,343,
		344,5,62,0,0,344,345,5,61,0,0,345,62,1,0,0,0,346,347,5,60,0,0,347,348,
		5,61,0,0,348,64,1,0,0,0,349,350,5,61,0,0,350,66,1,0,0,0,351,352,5,35,0,
		0,352,68,1,0,0,0,353,354,5,110,0,0,354,355,5,111,0,0,355,356,5,116,0,0,
		356,70,1,0,0,0,357,358,5,97,0,0,358,359,5,110,0,0,359,360,5,100,0,0,360,
		72,1,0,0,0,361,362,5,111,0,0,362,363,5,114,0,0,363,74,1,0,0,0,364,365,
		5,120,0,0,365,366,5,111,0,0,366,367,5,114,0,0,367,76,1,0,0,0,368,369,5,
		58,0,0,369,370,5,61,0,0,370,78,1,0,0,0,371,372,5,43,0,0,372,80,1,0,0,0,
		373,374,5,45,0,0,374,82,1,0,0,0,375,376,5,42,0,0,376,84,1,0,0,0,377,378,
		5,47,0,0,378,86,1,0,0,0,379,380,5,109,0,0,380,381,5,111,0,0,381,382,5,
		100,0,0,382,88,1,0,0,0,383,384,5,46,0,0,384,385,5,97,0,0,385,386,5,110,
		0,0,386,387,5,100,0,0,387,388,5,46,0,0,388,90,1,0,0,0,389,390,5,46,0,0,
		390,391,5,111,0,0,391,392,5,114,0,0,392,393,5,46,0,0,393,92,1,0,0,0,394,
		395,5,46,0,0,395,396,5,110,0,0,396,397,5,111,0,0,397,398,5,116,0,0,398,
		399,5,46,0,0,399,94,1,0,0,0,400,401,5,46,0,0,401,402,5,120,0,0,402,403,
		5,111,0,0,403,404,5,114,0,0,404,405,5,46,0,0,405,96,1,0,0,0,406,407,5,
		38,0,0,407,98,1,0,0,0,408,409,5,40,0,0,409,100,1,0,0,0,410,411,5,41,0,
		0,411,102,1,0,0,0,412,413,5,91,0,0,413,104,1,0,0,0,414,415,5,93,0,0,415,
		106,1,0,0,0,416,417,5,44,0,0,417,108,1,0,0,0,418,420,2,48,57,0,419,418,
		1,0,0,0,420,421,1,0,0,0,421,419,1,0,0,0,421,422,1,0,0,0,422,423,1,0,0,
		0,423,425,5,46,0,0,424,426,2,48,57,0,425,424,1,0,0,0,426,427,1,0,0,0,427,
		425,1,0,0,0,427,428,1,0,0,0,428,110,1,0,0,0,429,434,5,34,0,0,430,433,8,
		5,0,0,431,433,3,113,56,0,432,430,1,0,0,0,432,431,1,0,0,0,433,436,1,0,0,
		0,434,432,1,0,0,0,434,435,1,0,0,0,435,437,1,0,0,0,436,434,1,0,0,0,437,
		438,5,34,0,0,438,112,1,0,0,0,439,440,5,92,0,0,440,441,7,6,0,0,441,114,
		1,0,0,0,442,444,3,121,60,0,443,442,1,0,0,0,443,444,1,0,0,0,444,446,1,0,
		0,0,445,447,3,119,59,0,446,445,1,0,0,0,447,448,1,0,0,0,448,446,1,0,0,0,
		448,449,1,0,0,0,449,116,1,0,0,0,450,451,7,7,0,0,451,118,1,0,0,0,452,455,
		3,117,58,0,453,455,7,8,0,0,454,452,1,0,0,0,454,453,1,0,0,0,455,120,1,0,
		0,0,456,457,7,9,0,0,457,122,1,0,0,0,18,0,127,133,140,150,158,162,165,178,
		186,194,421,427,432,434,443,448,454,1,0,1,0
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
} // namespace KSPCompiler.Infrastructures.Parser.Antlr
