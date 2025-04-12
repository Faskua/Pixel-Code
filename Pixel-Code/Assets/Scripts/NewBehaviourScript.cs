using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public int row;
    public int col;
    public int Dx;
    public int Dy;
    public int Distance;
    public int height;
    public int width;
    private Wall wall; 

    public void PaintPixel(){
        if(wall.IsPosible(row, col)){
            wall.Row = row;
            wall.Column = col;
        }
        wall.PaintPixel("Blue");
    } 

    public void PaintLine(){
        Instruction line = new DrawLine(wall, Dx, Dy, Distance);
        wall.PaintInstruction(line);
    }

    public void PaintRectangle(){
        Instruction rectangle = new DrawRectangle(wall,Dx,Dy,Distance,width,height);
        wall.PaintInstruction(rectangle);
    }

    void Start()
    {
        wall = GameObject.FindGameObjectWithTag("WALL").GetComponent<Wall>();
    }
}
