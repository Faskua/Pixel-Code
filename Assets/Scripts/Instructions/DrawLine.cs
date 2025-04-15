using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : Instruction
{
    public int Distance { get; private set; }
    public int Dx { get; private set; }
    public int Dy { get; private set; }
    public DrawLine(Wall wall, int dx, int dy, int distance) : base(wall){
        Distance = distance;

        if(dx < 0) Dx = -1;
        else if(dx > 0) Dx = 1;
        else Dx = 0;

        if(dy < 0) Dy = -1;
        else if(dy > 0) Dy = 1;
        else Dy = 0;
    }

    public override void Paint()
    {
        for (int i = 0; i < Distance; i++)
        {
            int newRow = Wall.Row + Dy, newCol =  Wall.Column + Dx;
            Wall.PaintPixel();
            if(!Wall.IsPosible(newRow, newCol)) return;
            Wall.Row = newRow;
            Wall.Column = newCol;
        }
    }
}
