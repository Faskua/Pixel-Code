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
    public Expression Row1 { get; private set; }
    public Expression Col1 { get; private set; }
    public Expression Row2 { get; private set; }
    public Expression Col2 { get; private set; }
    public GetColorCountExpression(IDType type, CodeLocation location, Wall wall, string color, Expression x1, Expression y1, Expression x2, Expression y2) : base(type, location, wall){
        Color = color;
    }
    public override bool Validate(Global Global) => Type == IDType.GetColorCount;
    public override object? Evaluate(Global Global){
        int col1 = (int)Col1.Evaluate(Global),  col2 = (int)Col2.Evaluate(Global);
        int row1 = (int)Row1.Evaluate(Global), row2 = (int)Row2.Evaluate(Global);
        Function function = new GetColorCount(Wall, Color, col1, row1, col2, row2);
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
    public Expression Size { get; private set; }
    public IsBrushSizeExpression(IDType type, CodeLocation location, Wall wall, Expression size) : base(type, location, wall){
        Size = size;
    }
    public override bool Validate(Global Global) => Type == IDType.IsBrushSize;
    public override object? Evaluate(Global Global){
        Function function = new IsBrushSize(Wall, (int)Size.Evaluate(Global));
        return Wall.EvaluateFunction(function);
    }
} 

public class IsCanvasColorExpression : DSLExpression
{
    public string Color { get; private set; }
    public Expression Horizontal { get; private set; }
    public Expression Vertical { get; private set; }
    public IsCanvasColorExpression(IDType type, CodeLocation location, Wall wall, string color, Expression horizontal, Expression vertical) : base(type, location, wall){
        Color = color;
        Horizontal = horizontal;
        Vertical = vertical;
    }
    public override bool Validate(Global Global) => Type == IDType.IsCanvasColor;
    public override object? Evaluate(Global Global){
        Function function = new IsCanvasColor(Wall, Color, (int)Horizontal.Evaluate(Global), (int)Vertical.Evaluate(Global));
        return Wall.EvaluateFunction(function);
    }
}