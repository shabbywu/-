using System;
using MoonSharp.Interpreter.Debugging;

namespace MoonSharp.Interpreter.Tree;

internal class Token
{
	public readonly int SourceId;

	public readonly int FromCol;

	public readonly int ToCol;

	public readonly int FromLine;

	public readonly int ToLine;

	public readonly int PrevCol;

	public readonly int PrevLine;

	public readonly TokenType Type;

	public string Text { get; set; }

	public Token(TokenType type, int sourceId, int fromLine, int fromCol, int toLine, int toCol, int prevLine, int prevCol)
	{
		Type = type;
		SourceId = sourceId;
		FromLine = fromLine;
		FromCol = fromCol;
		ToCol = toCol;
		ToLine = toLine;
		PrevCol = prevCol;
		PrevLine = prevLine;
	}

	public override string ToString()
	{
		string arg = (Type.ToString() + "                                                      ").Substring(0, 16);
		string text = $"{FromLine}:{FromCol}-{ToLine}:{ToCol}";
		text = (text + "                                                      ").Substring(0, 10);
		return string.Format("{0}  - {1} - '{2}'", arg, text, Text ?? "");
	}

	public static TokenType? GetReservedTokenType(string reservedWord)
	{
		return reservedWord switch
		{
			"and" => TokenType.And, 
			"break" => TokenType.Break, 
			"do" => TokenType.Do, 
			"else" => TokenType.Else, 
			"elseif" => TokenType.ElseIf, 
			"end" => TokenType.End, 
			"false" => TokenType.False, 
			"for" => TokenType.For, 
			"function" => TokenType.Function, 
			"goto" => TokenType.Goto, 
			"if" => TokenType.If, 
			"in" => TokenType.In, 
			"local" => TokenType.Local, 
			"nil" => TokenType.Nil, 
			"not" => TokenType.Not, 
			"or" => TokenType.Or, 
			"repeat" => TokenType.Repeat, 
			"return" => TokenType.Return, 
			"then" => TokenType.Then, 
			"true" => TokenType.True, 
			"until" => TokenType.Until, 
			"while" => TokenType.While, 
			_ => null, 
		};
	}

	public double GetNumberValue()
	{
		if (Type == TokenType.Number)
		{
			return LexerUtils.ParseNumber(this);
		}
		if (Type == TokenType.Number_Hex)
		{
			return LexerUtils.ParseHexInteger(this);
		}
		if (Type == TokenType.Number_HexFloat)
		{
			return LexerUtils.ParseHexFloat(this);
		}
		throw new NotSupportedException("GetNumberValue is supported only on numeric tokens");
	}

	public bool IsEndOfBlock()
	{
		TokenType type = Type;
		if (type == TokenType.Eof || (uint)(type - 6) <= 2u || type == TokenType.Until)
		{
			return true;
		}
		return false;
	}

	public bool IsUnaryOperator()
	{
		if (Type != TokenType.Op_MinusOrSub && Type != TokenType.Not)
		{
			return Type == TokenType.Op_Len;
		}
		return true;
	}

	public bool IsBinaryOperator()
	{
		switch (Type)
		{
		case TokenType.And:
		case TokenType.Or:
		case TokenType.Op_Equal:
		case TokenType.Op_LessThan:
		case TokenType.Op_LessThanEqual:
		case TokenType.Op_GreaterThanEqual:
		case TokenType.Op_GreaterThan:
		case TokenType.Op_NotEqual:
		case TokenType.Op_Concat:
		case TokenType.Op_Pwr:
		case TokenType.Op_Mod:
		case TokenType.Op_Div:
		case TokenType.Op_Mul:
		case TokenType.Op_MinusOrSub:
		case TokenType.Op_Add:
			return true;
		default:
			return false;
		}
	}

	internal SourceRef GetSourceRef(bool isStepStop = true)
	{
		return new SourceRef(SourceId, FromCol, ToCol, FromLine, ToLine, isStepStop);
	}

	internal SourceRef GetSourceRef(Token to, bool isStepStop = true)
	{
		return new SourceRef(SourceId, FromCol, to.ToCol, FromLine, to.ToLine, isStepStop);
	}

	internal SourceRef GetSourceRefUpTo(Token to, bool isStepStop = true)
	{
		return new SourceRef(SourceId, FromCol, to.PrevCol, FromLine, to.PrevLine, isStepStop);
	}
}
