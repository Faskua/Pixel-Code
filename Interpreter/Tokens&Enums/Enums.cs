public enum TokenType
{
    Int,    String,
    True,   False, 
    Identifier,     Label,

    Plus,   Minus,
    Mult,   Div,
    Pow,    Mod,

    And,    Or,
    Less,   Greater,
    LessEqual,  GreaterEqual,
    Equal,

    Assignation,    Comma,
    LParen, RParen,
    LBracket,   RBracket,
      

    Spawn,  Color, 
    Size,   DrawLine,
    DrawCircle,     DrawRectangle,
    Fill,   GetActualX,
    GetActualY,     GetCanvasSize,
    GetColorCount,  IsBrushColor,
    IsBrushSize,    IsCanvasColor,
    GoTo,  

    Unknown
}

public enum IDType{
    Numeric,    Boolean,
    String,     Function
}

public enum Color{
    Red,    Blue,
    Green,  Yellow,
    Orange, Purple,
    Black,  White,
    Transparent
}