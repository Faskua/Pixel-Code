using System; 

public class IsBrushColor : Function
{
    public string Color { get; private set; }
    public IsBrushColor(Wall wall, string color) : base(wall){
        Color = color;
    }
    public override int Evaluate(){
        if(Wall.Color == Color) return 1;
        return 0;
    }
}

public class IsBrushSize : Function
{
    public int Size { get; private set; }
    public IsBrushSize(Wall wall, int size) : base(wall){
        Size = size;
    }
    public override int Evaluate(){
        if(Wall.BrushSize == Size) return 1;
        return 0;
    }
}

public class IsCanvasColor : Function
{
    public string Color { get; private set; }
    public int Horizontal { get; private set; }
    public int Vertical { get; private set; }
    public IsCanvasColor(Wall wall, string color, int horizontal, int vertical) : base(wall){
        Color = color;
        Horizontal = horizontal;
        Vertical = vertical;
    }
    public override int Evaluate(){
        int Row = Wall.Row + Vertical, Column = Wall.Column + Horizontal;
        if(!Wall.IsPosible(Row, Column)) return 0;
        if(Wall.GetPixelColor(Row, Column) == Color) return 1;
        return 0;
    }
}