using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawRectangle : Instruction
{
    public int Dx { get; private set; }
    public int Dy { get; private set; }
    public int Distance { get; private set; }
    public int Height { get; private set; }
    public int Width { get; private set; }
    public DrawRectangle(Wall wall, int dx, int dy, int distance, int width, int height) : base(wall){
        Distance = distance;
        Height = height % 2 == 0 ? height+1 : height;
        Width = width % 2 == 0 ? width+1 : width;

        if(dx < 0) Dx = -1;
        else if(dx > 0) Dx = 1;
        else Dx = 0;

        if(dy < 0) Dy = -1;
        else if(dy > 0) Dy = 1;
        else Dy = 0;
    }

    public override void Paint()
    {
        int centerRow = Wall.Row + Dy * Distance, centerColumn = Wall.Column + Dx * Distance;
        int LeftR = centerRow, LeftC = centerColumn - Width / 2;
        int RightR = centerRow, RightC = centerColumn + Width / 2;
        int UpR = centerRow - Height / 2, UpC = centerColumn;
        int DownR = centerRow + Height / 2, DownC = centerColumn;

        Instruction forUse;

        if(Wall.IsPosible(LeftR, LeftC)){
            Wall.Row = LeftR;
            Wall.Column = LeftC;
            forUse = new DrawLine(Wall, 0, -1, Height / 2);
            Wall.PaintInstruction(forUse);
            Wall.Row = LeftR;
            Wall.Column = LeftC;
            forUse = new DrawLine(Wall, 0,  1, Height / 2);
            Wall.PaintInstruction(forUse);
        }
        if(Wall.IsPosible(RightR, RightC)){
            Wall.Row = RightR;
            Wall.Column = RightC;
            forUse = new DrawLine(Wall, 0, -1, Height / 2);
            Wall.PaintInstruction(forUse);
            Wall.Row = RightR;
            Wall.Column = RightC;
            forUse = new DrawLine(Wall, 0,  1, Height / 2);
            Wall.PaintInstruction(forUse);
        }if(Wall.IsPosible(UpR, UpC)){
            Wall.Row = UpR;
            Wall.Column = UpC;
            forUse = new DrawLine(Wall, -1, 0, (Width / 2) + 1 );
            Wall.PaintInstruction(forUse);
            Wall.Row = UpR;
            Wall.Column = UpC;
            forUse = new DrawLine(Wall, 1,  0, (Width / 2) + 1 );
            Wall.PaintInstruction(forUse);
        }if(Wall.IsPosible(DownR, DownC)){
            Wall.Row = DownR;
            Wall.Column = DownC;
            forUse = new DrawLine(Wall, -1, 0, (Width / 2) + 1 );
            Wall.PaintInstruction(forUse);
            Wall.Row = DownR;
            Wall.Column = DownC;
            forUse = new DrawLine(Wall, 1,  0, (Width / 2) + 1 );
            Wall.PaintInstruction(forUse);
        }
        Wall.Row = centerRow;
        Wall.Column = centerColumn;
    }
}

