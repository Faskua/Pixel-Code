using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPixel : Instruction
{
    public string Color { get; private set; }
    public int Row { get; private set; }
    public int Column { get; private set; }
    public DrawPixel(Wall wall, string color, int row, int column) : base(wall){
        Color = color;
        Row = row;
        Column = column;
    }
    public override void Paint(){
        if(!Wall.IsPosible(Row, Column)) return;
        Wall.Row = Row;
        Wall.Column = Column;
        Wall.Color = Color;
        Wall.PaintPixel();
    }
}
