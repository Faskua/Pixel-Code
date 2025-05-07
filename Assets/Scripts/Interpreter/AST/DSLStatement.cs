using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class DSLStatement : Statement
{
    public Wall Wall { get; private set; }
    public DSLStatement(IDType type, CodeLocation location, Wall wall) : base(type, location){
        Wall = wall;
    }
}

public class SpawnStatement : DSLStatement
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public SpawnStatement(IDType type, CodeLocation location, int x, int y, Wall wall) : base(type, location, wall){
        X = x;
        Y = y;
    }
    public override bool Validate(Global Global) => Type == IDType.Spawn;
    public override void Evaluate(Global Global){
        Command command = new Spawn(Wall, X, Y);
        Wall.ObeyOrder(command);
    }
}

public class ColorStatement : DSLStatement
{
    public string Color { get; private set; }
    public ColorStatement(IDType type, CodeLocation location, string color, Wall wall) : base(type, location, wall){
        Color = color;
    }
    public override bool Validate(Global Global) => Type == IDType.Color;
    public override void Evaluate(Global Global){
        Command command = new ColorCommand(Wall, Color);
        Wall.ObeyOrder(command);
    }
}

public class SizeStatement : DSLStatement
{
    public int Size { get; private set; }
    public SizeStatement(IDType type, CodeLocation location, int size, Wall wall) : base(type, location, wall){
        Size = size;
    }
    public override bool Validate(Global Global) => Type == IDType.Size;
    public override void Evaluate(Global Global){
        Command command = new Size(Wall, Size);
        Wall.ObeyOrder(command);
    }
}

public class LineStatement : DSLStatement
{
    public int DX { get; private set; }
    public int DY { get; private set; }
    public int Distance { get; private set; }
    public LineStatement(IDType type, CodeLocation location, int dx, int dy, int distance, Wall wall) : base(type, location, wall){
        DX = dx;
        DY = dy;
        Distance = distance;
    }
    public override bool Validate(Global Global) => Type == IDType.DrawLine;
    public override void Evaluate(Global Global){
        Instruction instruction = new DrawLine(Wall, DX, DY, Distance);
        Wall.PaintInstruction(instruction);
    }
}

public class CircleStatement : DSLStatement
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public int Radius { get; private set; }
    public CircleStatement(IDType type, CodeLocation location, int x, int y, int radius, Wall wall) : base(type, location, wall){
        X = x;
        Y = y;
        Radius = radius;
    }
    public override bool Validate(Global Global) => Type == IDType.DrawCircle;
    public override void Evaluate(Global Global){
        Instruction instruction = new DrawCircle(Wall, X, Y, Radius);
        Wall.PaintInstruction(instruction);
    }
}

public class RectangleStatement : DSLStatement
{
    public int DX { get; private set; }
    public int DY { get; private set; }
    public int Distance { get; private set; }
    public int Height { get; private set; }
    public int Width { get; private set; }
    public RectangleStatement(IDType type, CodeLocation location, int dx, int dy, int distance, int width, int height, Wall wall) : base(type, location, wall){
        DX = dx;
        DY = dy;
        Distance = distance;
        Width = width;
        Height = height;
    }
    public override bool Validate(Global Global) => Type == IDType.DrawRectangle;
    public override void Evaluate(Global Global){
        Instruction instruction = new DrawRectangle(Wall, DX, DY, Distance, Width, Height);
        Wall.PaintInstruction(instruction);
    }
}

public class FillStatement : DSLStatement
{
    public FillStatement(IDType type, CodeLocation location, Wall wall) : base(type, location, wall){}
    public override bool Validate(Global Global) => Type == IDType.Fill;
    public override void Evaluate(Global Global){
        Instruction instruction = new Fill(Wall);
        Wall.PaintInstruction(instruction);
    }
}
