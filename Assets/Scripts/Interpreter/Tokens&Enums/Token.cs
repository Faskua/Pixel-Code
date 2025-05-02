using System.Collections.Generic;

public class Token
{
    public string Value { get; private set; }
    public TokenType Type { get; private set; }
    public CodeLocation Location { get; private set; }

    public Token(string value, TokenType type, CodeLocation location){
        Value = value;
        Type = type;
        Location = location;
    }

    public static Dictionary<string, TokenType> Types = new Dictionary<string, TokenType>(){
        {"true", TokenType.True},
        {"false", TokenType.False},

        {"+", TokenType.Plus},
        {"-", TokenType.Minus},
        {"*", TokenType.Mult},
        {"/", TokenType.Div},
        {"**", TokenType.Pow},
        {"%", TokenType.Mod},

        {"&&", TokenType.And},
        {"||", TokenType.Or},
        {"<", TokenType.Less},
        {">", TokenType.Greater},
        {"<=", TokenType.LessEqual},
        {">=", TokenType.GreaterEqual},
        {"==", TokenType.Equal},

        {"<-", TokenType.Assignation},
        {"(", TokenType.LParen},
        {")", TokenType.RParen},
        {"[", TokenType.LBracket},
        {"]", TokenType.RBracket},
        {",", TokenType.Comma},

        {"Red", TokenType.PixelColor},
        {"Blue", TokenType.PixelColor},
        {"Green", TokenType.PixelColor},
        {"Yellow", TokenType.PixelColor},
        {"Orange", TokenType.PixelColor},
        {"Purple", TokenType.PixelColor},
        {"Black", TokenType.PixelColor},
        {"White", TokenType.PixelColor},
        {"Transparent", TokenType.PixelColor},

        {"Spawn", TokenType.Spawn},
        {"Color", TokenType.Color},
        {"Size", TokenType.Size},
        {"DrawLine", TokenType.DrawLine},
        {"DrawCircle", TokenType.DrawCircle},
        {"DrawRectangle", TokenType.DrawRectangle},
        {"Fill", TokenType.Fill},
        {"GetActualX", TokenType.GetActualX},
        {"GetActualY", TokenType.GetActualY},
        {"GetCanvasSize", TokenType.GetCanvasSize},
        {"GetColorCount", TokenType.GetColorCount},
        {"IsBrushColor", TokenType.IsBrushColor},
        {"IsBrushSize", TokenType.IsBrushSize},
        {"IsCanvasColor", TokenType.IsCanvasColor},
        {"GoTo", TokenType.GoTo},
    };
}

public class CodeLocation
{
    public int Line { get; private set; }
    public int Column { get; private set; }

    public CodeLocation(int line, int column){
        Line = line;
        Column = column;
    }
}