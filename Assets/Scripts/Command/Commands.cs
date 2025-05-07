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
    public override void Order(){
        Wall.Row = Y;
        Wall.Column = X;
        Wall.WallE.GetComponent<Transform>().position = Wall.Pixels[Wall.Row, Wall.Column].GetComponent<Transform>().position;
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