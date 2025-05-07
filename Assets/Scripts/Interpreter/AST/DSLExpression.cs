using System;
using System.Collections.Generic;

public abstract class DSLExpression : Expression
{
    public Wall Wall { get; private set; }
    public DSLExpression(IDType type, CodeLocation location, Wall wall) : base(type, location){
        Wall = wall;
    }
}

public class GetActualXExpression : DSLExpression
{
    public GetActualXExpression(IDType type, CodeLocation location, Wall wall) : base(type, location, wall){ 
    }
    public override bool Validate(Global Global) => Type == IDType.GetActualX;
    public override object? Evaluate(Global Global){
        Function function = new GetActualX(Wall);
        return Wall.EvaluateFunction(function);
    } 
}

public class GetActualYExpression : DSLExpression
{
    public GetActualYExpression(IDType type, CodeLocation location, Wall wall) : base(type, location, wall){
    }
    public override bool Validate(Global Global) => Type == IDType.GetActualY;
    public override object? Evaluate(Global Global){
        Function function = new GetActualY(Wall);
        return Wall.EvaluateFunction(function);
    }
}

public class GetCanvasSizeExpression : DSLExpression
{
    public GetCanvasSizeExpression(IDType type, CodeLocation location, Wall wall) : base(type, location, wall){
    }
    public override bool Validate(Global Global) => Type == IDType.GetCanvasSize;
    public override object? Evaluate(Global Global){
        Function function = new GetCanvasSize(Wall);
        return Wall.EvaluateFunction(function);
    }
}

public class GetColorCountExpression : DSLExpression
{
    public string Color { get; private set; }
    public int Row1 { get; private set; }
    public int Col1 { get; private set; }
    public int Row2 { get; private set; }
    public int Col2 { get; private set; }
    public GetColorCountExpression(IDType type, CodeLocation location, Wall wall, string color, int x1, int y1, int x2, int y2) : base(type, location, wall){
        Color = color;
        Row1 = Math.Min(y1, y2);
        Col1 = Math.Min(x1, x2);
        Row2 = Math.Max(y1, y2);
        Col2 = Math.Max(x1, x2);
    }
    public override bool Validate(Global Global) => Type == IDType.GetColorCount;
    public override object? Evaluate(Global Global){
        Function function = new GetColorCount(Wall, Color, Col1, Row1, Col2, Row2);
        return Wall.EvaluateFunction(function);
    }
}

public class IsBrushColorExpression : DSLExpression
{
    public string Color { get; private set; }
    public IsBrushColorExpression(IDType type, CodeLocation location, Wall wall, string color) : base(type, location, wall){
        Color = color;
    }
    public override bool Validate(Global Global) => Type == IDType.IsBrushColor;
    public override object? Evaluate(Global Global){
        Function function = new IsBrushColor(Wall, Color);
        return Wall.EvaluateFunction(function);
    }
}

public class IsBrushSizeExpression : DSLExpression
{
    public int Size { get; private set; }
    public IsBrushSizeExpression(IDType type, CodeLocation location, Wall wall, int size) : base(type, location, wall){
        Size = size;
    }
    public override bool Validate(Global Global) => Type == IDType.IsBrushSize;
    public override object? Evaluate(Global Global){
        Function function = new IsBrushSize(Wall, Size);
        return Wall.EvaluateFunction(function);
    }
} 

public class IsCanvasColorExpression : DSLExpression
{
    public string Color { get; private set; }
    public int Horizontal { get; private set; }
    public int Vertical { get; private set; }
    public IsCanvasColorExpression(IDType type, CodeLocation location, Wall wall, string color, int horizontal, int vertical) : base(type, location, wall){
        Color = color;
        Horizontal = horizontal;
        Vertical = vertical;
    }
    public override bool Validate(Global Global) => Type == IDType.IsCanvasColor;
    public override object? Evaluate(Global Global){
        Function function = new IsCanvasColor(Wall, Color, Horizontal, Vertical);
        return Wall.EvaluateFunction(function);
    }
}