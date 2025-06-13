using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintedPixel
{
    public int Row { get; private set; }
    public int Column { get; private set; }
    public string Color { get; private set; }
    public Wall Wall;

    public PaintedPixel(int r, int c, string color, Wall w) {
        Row = r;
        Column = c;
        Color = color;
        Wall = w;
    }

    public void Paint()
    {
        Wall.Pixels[Row, Column].GetComponent<PixelUN>().Change(Color);
        Wall.WallE.GetComponent<Transform>().position = Wall.Pixels[Row, Column].GetComponent<Transform>().position;
    }
}
