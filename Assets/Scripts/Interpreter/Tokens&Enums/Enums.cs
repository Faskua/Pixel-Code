public enum TokenType
{
    Int,    PixelColor,
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

    Unknown,        EOL
}

public enum IDType{
    Numeric,    Boolean,
    ColorPixel,     Declaration,

    Spawn,      Color,
    Size,       DrawLine,
    DrawRectangle,      DrawCircle,
    Fill,       
    
    GetActualX,     GetActualY,
    GetCanvasSize,      GetColorCount,
    IsBrushColor,       IsBrushSize,
    IsCanvasColor,      GoTo
}

public enum PixelColor{
    Red,    Blue,
    Green,  Yellow,
    Orange, Purple,
    Black,  White,
    Transparent
}