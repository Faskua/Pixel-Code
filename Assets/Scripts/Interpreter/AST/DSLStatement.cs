using System;
using System.Collections.Generic;

public class SpawnStatement : Statement
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public SpawnStatement(IDType type, CodeLocation location, int x, int y) : base(type, location){
        X = x;
        Y = y;
    }
    public override bool Validate(Global Global) => Type == IDType.Spawn;
    public override void Evaluate(Global Global){}
}

public class ColorStatement : Statement
{
    public string Color { get; private set; }
    public ColorStatement(IDType type, CodeLocation location, string color) : base(type, location){
        Color = color;
    }
    public override bool Validate(Global Global) => Type == IDType.Color;
    public override void Evaluate(Global Global){}
}

public class SizeStatement : Statement
{
    public int Size { get; private set; }
    public SizeStatement(IDType type, CodeLocation location, int size) : base(type, location){
        Size = size;
    }
    public override bool Validate(Global Global) => Type == IDType.Size;
    public override void Evaluate(Global Global){}
}

public class LineStatement : Statement
{
    public int DX { get; private set; }
    public int DY { get; private set; }
    public int Distance { get; private set; }
    public LineStatement(IDType type, CodeLocation location, int dx, int dy, int distance) : base(type, location){
        DX = dx;
        DY = dy;
        Distance = distance;
    }
    public override bool Validate(Global Global) => Type == IDType.DrawLine;
    public override void Evaluate(Global Global){}
}

public class CircleStatement : Statement
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public int Radius { get; private set; }
    public CircleStatement(IDType type, CodeLocation location, int x, int y, int radius) : base(type, location){
        X = x;
        Y = y;
        Radius = radius;
    }
    public override bool Validate(Global Global) => Type == IDType.DrawCircle;
    public override void Evaluate(Global Global){}
}

public class RectangleStatement : Statement
{
    public int DX { get; private set; }
    public int DY { get; private set; }
    public int Distance { get; private set; }
    public int Heigth { get; private set; }
    public int Width { get; private set; }
    public RectangleStatement(IDType type, CodeLocation location, int dx, int dy, int distance, int heigth, int width) : base(type, location){
        DX = dx;
        DY = dy;
        Distance = distance;
        Heigth = heigth;
        Width = width;
    }
    public override bool Validate(Global Global) => Type == IDType.DrawRectangle;
    public override void Evaluate(Global Global){}
}

public class FillStatement : Statement
{
    public FillStatement(IDType type, CodeLocation location) : base(type, location){}
    public override bool Validate(Global Global) => Type == IDType.Fill;
    public override void Evaluate(Global Global){}
}
