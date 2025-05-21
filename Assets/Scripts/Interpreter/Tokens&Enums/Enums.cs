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

    EOF,        EOL,    Unknown
}

public enum IDType{
    Numeric,    Boolean,
    ColorPixel,     Declaration,
    Block,      Variable,

    Spawn,      Color,
    Size,       
    
    DrawLine,       Fill,
    DrawRectangle,      DrawCircle,       
    
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