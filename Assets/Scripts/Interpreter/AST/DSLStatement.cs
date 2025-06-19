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
    public Expression X { get; private set; }
    public Expression Y { get; private set; }
    public SpawnStatement(IDType type, CodeLocation location, Expression x, Expression y, Wall wall) : base(type, location, wall){
        X = x;
        Y = y;
    }
    public override bool Validate(Global Global) => Type == IDType.Spawn;
    public override void Evaluate(Global Global){
        int x = (int)X.Evaluate(Global), y = (int)Y.Evaluate(Global);
        if (x >= Wall.Pixels.GetLength(0) || x < 0 || y >= Wall.Pixels.GetLength(0) || y < 0) Global.AddError(Location.Line, $"OutofCanvasException at line: {Location.Line}, column: {Location.Column}");
        else
        {
            Command command = new Spawn(Wall, x, y);
            Wall.ObeyOrder(command);
        }
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
    public Expression Size { get; private set; }
    public SizeStatement(IDType type, CodeLocation location, Expression size, Wall wall) : base(type, location, wall){
        Size = size;
    }
    public override bool Validate(Global Global) => Type == IDType.Size;
    public override void Evaluate(Global Global){
        int s = (int)Size.Evaluate(Global);
        if (s < 1) Global.AddError(Location.Line, $"InvalidArgumentException at line: {Location.Line}, column: {Location.Column}");
        else
        {
            Command command = new Size(Wall, s);
            Wall.ObeyOrder(command);            
        }
    }
}

public class LineStatement : DSLStatement
{
    public Expression DX { get; private set; }
    public Expression DY { get; private set; }
    public Expression Distance { get; private set; }
    public LineStatement(IDType type, CodeLocation location, Expression dx, Expression dy, Expression distance, Wall wall) : base(type, location, wall){
        DX = dx;
        DY = dy;
        Distance = distance;
    }
    public override bool Validate(Global Global) => Type == IDType.DrawLine;
    public override void Evaluate(Global Global){
        int d = (int)Distance.Evaluate(Global);
        if (d < 1) Global.AddError(Location.Line, $"InvalidArgumentException at line: {Location.Line}, column: {Location.Column}");
        else
        {
            Instruction instruction = new DrawLine(Wall, (int)DX.Evaluate(Global), (int)DY.Evaluate(Global), d);
            Wall.PaintInstruction(instruction);            
        }
    }
}

public class CircleStatement : DSLStatement
{
    public Expression X { get; private set; }
    public Expression Y { get; private set; }
    public Expression Radius { get; private set; }
    public CircleStatement(IDType type, CodeLocation location, Expression x, Expression y, Expression radius, Wall wall) : base(type, location, wall){
        X = x;
        Y = y;
        Radius = radius;
    }
    public override bool Validate(Global Global) => Type == IDType.DrawCircle;
    public override void Evaluate(Global Global){
        int x = (int)X.Evaluate(Global), y = (int)Y.Evaluate(Global), r = (int)Radius.Evaluate(Global);
        if (r < 1 || x < 0 || x >= Wall.Pixels.GetLength(0) || y < 0 || y >= Wall.Pixels.GetLength(0)) Global.AddError(Location.Line, $"InvalidArgumentException at line: {Location.Line}, column: {Location.Column}");
        else
        {
            Instruction instruction = new DrawCircle(Wall, x, y, r);
            Wall.PaintInstruction(instruction);            
        }
    }
}

public class RectangleStatement : DSLStatement
{
    public Expression DX { get; private set; }
    public Expression DY { get; private set; }
    public Expression Distance { get; private set; }
    public Expression Height { get; private set; }
    public Expression Width { get; private set; }
    public RectangleStatement(IDType type, CodeLocation location, Expression dx, Expression dy, Expression distance, Expression width, Expression height, Wall wall) : base(type, location, wall){
        DX = dx;
        DY = dy;
        Distance = distance;
        Width = width;
        Height = height;
    }
    public override bool Validate(Global Global) => Type == IDType.DrawRectangle;
    public override void Evaluate(Global Global){
        int d = (int)Distance.Evaluate(Global), h = (int)Height.Evaluate(Global), w = (int)Width.Evaluate(Global);
        if (d < 0 || h < 0 || w < 0) Global.AddError(Location.Line, $"InvalidArgumentException at line: {Location.Line}, column: {Location.Column}");
        else
        {
            Instruction instruction = new DrawRectangle(Wall, (int)DX.Evaluate(Global), (int)DY.Evaluate(Global), d, w, h);
            Wall.PaintInstruction(instruction);
        }
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
