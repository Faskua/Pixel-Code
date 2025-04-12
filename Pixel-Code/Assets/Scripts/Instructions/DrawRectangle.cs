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
        int upLeftR = centerRow - Height / 2, upLeftC = centerColumn - Width / 2;
        int downRightR = centerRow + Height / 2, downRightC = centerColumn + Width / 2;

        Instruction LineUp = new DrawLine(Wall, 1, 0, Width) ;
        Instruction LineDown = new DrawLine(Wall, -1, 0, Width);
        Instruction LineLeft = new DrawLine(Wall, 0, 1, Height);
        Instruction LineRight = new DrawLine(Wall, 0, -1, Height);

        if(Wall.IsPosible(upLeftR, upLeftC)){
            Wall.Row = upLeftR;
            Wall.Column = upLeftC;
            Wall.PaintInstruction(LineUp);
            Wall.PaintInstruction(LineLeft);
        }
        if(Wall.IsPosible(downRightR, downRightC)){
            Wall.Row = downRightR;
            Wall.Column = downRightC;
            Wall.PaintInstruction(LineDown);
            Wall.PaintInstruction(LineRight);
        }
        Wall.Row = centerRow;
        Wall.Column = centerColumn;
    }
}

