using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCircle : Instruction
{
    public int Radius { get; private set; }
    public int CenterRow { get; private set; }
    public int CenterCol { get; private set; }
    public DrawCircle(Wall wall, int x, int y, int radius) : base(wall){
        CenterRow = y;
        CenterCol = x;
        Radius = radius;
    }
    public override void Paint(){
        int x = 0, y = Radius, parameter = 3 - 2 * Radius;
        while (x <= y)
        {
            Instruction forUse = new DrawPixel(Wall, Wall.Color, CenterRow + y, CenterCol + x);
            Wall.PaintInstruction(forUse);
            forUse = new DrawPixel(Wall, Wall.Color, CenterRow + y, CenterCol - x);
            Wall.PaintInstruction(forUse);
            forUse = new DrawPixel(Wall, Wall.Color, CenterRow - y, CenterCol + x);
            Wall.PaintInstruction(forUse);
            forUse = new DrawPixel(Wall, Wall.Color, CenterRow - y, CenterCol - x);
            Wall.PaintInstruction(forUse);
            forUse = new DrawPixel(Wall, Wall.Color, CenterRow + x, CenterCol + y);
            Wall.PaintInstruction(forUse);
            forUse = new DrawPixel(Wall, Wall.Color, CenterRow + x, CenterCol - y);
            Wall.PaintInstruction(forUse);
            forUse = new DrawPixel(Wall, Wall.Color, CenterRow - x, CenterCol + y);
            Wall.PaintInstruction(forUse);
            forUse = new DrawPixel(Wall, Wall.Color, CenterRow - x, CenterCol - y);
            Wall.PaintInstruction(forUse);

            if (parameter < 0)  parameter += 4 * x + 6;
            else
            {
                parameter += 4 * (x - y) + 10;
                y--;
            }
            x++;
        } 
    }
}
