using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Wall : MonoBehaviour
{
    public bool Created;
    public GameObject[,] Pixels;
    public GameObject Pixel;
    public GameObject WallE;
    public string Color;
    public int Row;
    public int Column;
    public int BrushSize;
    public Queue<PaintedPixel> paintedPixels;
    public int Size;
    private float PixelSize;
    private string[] colors = {"Red", "Blue", "Green", "Yellow", "Orange", "Purple", "Black", "White", "Transparent"};
    

    public void ChangeSize(int size)
    {
        Size = size;
        PixelSize = 1024 / size;
        GenerateWall();
        Row = 0;
        Column = 0;
        var transform = WallE.GetComponent<RectTransform>();
        transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, PixelSize);
        transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, PixelSize);

    }
    public void ChangeBrushSize(int size){
        if(BrushSize <= 0) return;
        if(size % 2 != 0) BrushSize = size;
        else BrushSize = size-1;
    }
    public void PaintPixel(){
        int halfBrush = BrushSize / 2;
        int upRow = Row - halfBrush, downRow = Row + halfBrush;
        int leftCol = Column - halfBrush, rightCol = Column + halfBrush;
        for (int nRow = upRow; nRow <= downRow; nRow++)
        {
            for (int nCol = leftCol; nCol <= rightCol; nCol++)
            {
                if (!IsPosible(nRow, nCol)) continue;
                var p = new PaintedPixel(nRow, nCol, Color, this);
                paintedPixels.Enqueue(p);
            }
        }
    }
    public string GetPixelColor(int row, int col) => Pixels[row,col].GetComponent<PixelUN>().Color;
    public void PaintInstruction(Instruction instruction) => instruction.Paint();
    public void ObeyOrder(Command command) => command.Order();
    public int EvaluateFunction(Function function) => function.Evaluate();
    public bool IsPosible(int row, int col) => row >= 0 && row < Size && col >= 0 && col < Size;
    public void Restart()
    {
        Color = "White";
        for (int i = 0; i < Pixels.GetLength(0); i++)
        {
            for (int j = 0; j < Pixels.GetLength(1); j++)
            {
                Row = i;
                Column = j;
                PaintPixel();
            }
        }
        Row = 0;
        Column = 0;
        Color = "Black";
    }

    void GenerateWall(){
        WallE.GetComponent<Transform>().position = new Vector2(-420, 0);
        if(Pixels != null){
            for (int row = 0; row < Pixels.GetLength(0); row++)
            {
                for (int col = 0; col < Pixels.GetLength(1); col++)
                {
                    Destroy(Pixels[row,col]);
                }
            }
        }
        
        gameObject.GetComponent<GridLayoutGroup>().cellSize = new Vector2(PixelSize, PixelSize);

        Pixels = new GameObject[Size,Size];
        for (int row = 0; row < Size; row++)
        {
            for (int column = 0; column < Size; column++)
            {
                Pixels[row, column] = Instantiate(Pixel, new Vector2(0, 0), Quaternion.identity);
                Pixels[row, column].GetComponent<Transform>().SetParent(gameObject.GetComponent<Transform>(), false);
                Pixels[row, column].GetComponent<PixelUN>().X = column;
                Pixels[row, column].GetComponent<PixelUN>().Y = row;
            }
        }
        Created = true;
    }
    void Start()
    {
        Color = "Black";
        BrushSize = 1;
        Created = false;
        paintedPixels = new Queue<PaintedPixel>();
    }
}
