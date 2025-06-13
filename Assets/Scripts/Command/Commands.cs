using System;
using UnityEngine;

public class Spawn : Command
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public Spawn(Wall wall, int x, int y) : base(wall){
        X = x;
        Y = y;
    }
    public override void Order()
    {
        if (X < Wall.Pixels.GetLength(1) && X >= 0 && Y < Wall.Pixels.GetLength(0) && Y >= 0)
        {
            Wall.Row = Y;
            Wall.Column = X;
            Wall.WallE.GetComponent<Transform>().position = Wall.Pixels[Wall.Row, Wall.Column].GetComponent<Transform>().position;
        }
        else Debug.Log($"no realiza el spawn, la x es {X} y la y es {Y}");
    }
}

public class ColorCommand : Command
{
    public string Color { get; private set; }
    public ColorCommand(Wall wall, string color) : base(wall){
        Color = color;
    }
    public override void Order(){
        Wall.Color = Color;
    }
}

public class Size : Command
{
    public int BrushSize { get; private set; }
    public Size(Wall wall, int size) : base(wall){
        BrushSize = size;
    }
    public override void Order(){
        Wall.ChangeBrushSize(BrushSize);
    }
}