using UnityEngine;

public class Fill : Instruction
{
    public int Row { get; private set; }
    public int Column { get; private set; }
    private int[] Drow = {1, 0, -1, 0, 1, -1, -1, 1};
    private int[] Dcol = {0, 1, 0, -1, 1, -1, 1, -1};
    public Fill(Wall wall) : base(wall){
        Row = wall.Row;
        Column = wall.Column;
    } 
    public override void Paint(){
        bool[,] mask = new bool[Wall.Pixels.GetLength(0), Wall.Pixels.GetLength(1)];
        string color = Wall.GetPixelColor(Row, Column);
        Paint(color, Row, Column, mask);
        Wall.Row = Row;
        Wall.Column = Column;
        Wall.WallE.GetComponent<Transform>().position = Wall.Pixels[Row, Column].GetComponent<Transform>().position;
    }
    private void Paint(string color, int row, int column, bool[,] mask){
        Instruction draw = new DrawPixel(Wall, Wall.Color, row, column);
        Wall.PaintInstruction(draw);
        mask[row,column] = true;
        for (int i = 0; i < 4; i++)
        {
            int newRow = row + Drow[i], newCol = column + Dcol[i];
            if(Wall.IsPosible(newRow, newCol) && !mask[newRow,newCol]){
                if(Wall.GetPixelColor(newRow,newCol) == color){
                    Paint(color, newRow, newCol, mask);
                }  
            }
        }
    }
}