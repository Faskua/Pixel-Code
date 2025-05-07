using System;
using System.Collections.Generic;
using UnityEngine;

public class Evaluator : MonoBehaviour
{
    public Wall Wall { get; private set; }
    public Global Global { get; private set; }
    public Evaluator(Wall wall, Global global){
        Wall = wall;
        Global = global;
    }
    public void Evaluate(List<Statement> Statements){
        if(Global.Errors.Count > 0){
            Debug.Log("Hay errores");
            return;
        }
        foreach (Statement statement in Statements)
        {
            if(!statement.Validate(Global)) continue;
            switch (statement.Type)
            {
                case IDType.DrawLine:
                    LineStatement linestat = (LineStatement)statement;
                    Instruction line = new DrawLine(Wall, linestat.DX, linestat.DY, linestat.Distance);
                    Wall.PaintInstruction(line);
                break;
                case IDType.DrawCircle:
                    CircleStatement circlestat = (CircleStatement)statement;
                    Instruction circle = new DrawCircle(Wall, circlestat.X, circlestat.Y, circlestat.Radius);
                    Wall.PaintInstruction(circle);
                break;
                case IDType.DrawRectangle:
                    RectangleStatement rectstat = (RectangleStatement)statement;
                    Instruction rect = new DrawRectangle(Wall, rectstat.DX, rectstat.DY, rectstat.Distance, rectstat.Height, rectstat.Width);
                    Wall.PaintInstruction(rect);
                break;
                case IDType.Fill:
                    Instruction fill = new Fill(Wall);
                    Wall.PaintInstruction(fill);
                break;
                case IDType.Spawn:
                    SpawnStatement spawnstat = (SpawnStatement)statement;
                    Command spawn = new Spawn(Wall, spawnstat.X, spawnstat.Y);
                    Wall.ObeyOrder(spawn);
                break;
                case IDType.Color:
                    ColorStatement colorstat = (ColorStatement)statement;
                    Command color = new ColorCommand(Wall, colorstat.Color);
                    Wall.ObeyOrder(color);
                break;
                case IDType.Size:
                    SizeStatement sizestat = (SizeStatement)statement;
                    Command size = new Size(Wall, sizestat.Size);
                    Wall.ObeyOrder(size);
                break;
                // case IDType.GetActualX:   case IDType.GetActualY:     case IDType.GetCanvasSize:      case IDType.GetColorCount:    case IDType.IsBrushColor:   case IDType.IsBrushSize:    case IDType.IsCanvasColor:
                //     Instruction instruction = (Instruction)statement.Evaluate(Global);
                //     Wall.PaintInstruction(instruction);
                // break;
                default:
                break;
            }
        }
    }
}
