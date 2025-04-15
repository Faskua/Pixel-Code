using System;

public class GetActualX : Function
{
    public GetActualX(Wall wall) : base(wall){}
    public override int Evaluate() => Wall.Column;
}

public class GetActualY : Function
{
    public GetActualY(Wall wall) : base(wall){}
    public override int Evaluate() => Wall.Row;
}

public class GetCanvasSize : Function
{
    public GetCanvasSize(Wall wall) : base(wall){}
    public override int Evaluate() => Wall.Pixels.GetLength(0);
}

public class GetColorCount : Function
{
    public string Color;
    public int Row1 { get; private set; }
    public int Col1 { get; private set; }
    public int Row2 { get; private set; }
    public int Col2 { get; private set; }
    public GetColorCount(Wall wall, string color, int x1, int y1, int x2, int y2) : base(wall){
        Color = color;
        Row1 = Math.Min(y1, y2);
        Col1 = Math.Min(x1, x2);
        Row2 = Math.Max(y1, y2);
        Col2 = Math.Max(x1, x2);
    }
    public override int Evaluate(){
        if(!Wall.IsPosible(Row1, Col1) || !Wall.IsPosible(Row2, Col2)) return 0;
        int sum = 0;
        for (int row = Row1; row <= Row2; row++)
        {
            for (int col = Col1; col <= Col2; col++)
            {
                if(Wall.GetPixelColor(row, col) == Color) sum++;
            }
        }
        return sum;
    }
}