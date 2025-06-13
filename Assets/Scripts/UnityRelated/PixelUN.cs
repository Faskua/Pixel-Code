using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PixelUN : MonoBehaviour
{
    public string Color;
    private Color color;
    public Text PixelCoords;
    public int X;
    public int Y;

    public void Change(string newcolor){
        Color = newcolor;
        switch(newcolor){
            case "Red":
                color = new Color32(247, 15, 11, 255);
            break;
            case "Blue":
                color = new Color32(7, 44, 215, 255);
            break;
            case "Green":
                color = new Color32(30, 170, 20, 255);
            break;
            case "Yellow":
                color = new Color32(244, 240, 9, 255);
            break;
            case "Orange":
                color = new Color32(255, 133, 26, 255);
            break;
            case "Purple":
                color = new Color32(152, 0, 255, 255);
            break;
            case "Black":
                color = new Color32(0, 0, 0, 255);
            break;
            case "White":
                color = new Color32(255, 255, 255, 255);
            break;
            case "Gray":
                color = new Color32(120, 120, 120, 255);
            break;
            case "Pink":
                color = new Color32(255, 0, 255, 255);
            break;
            case "LightBlue":
                color = new Color32(0, 200, 255, 255);
            break;
            case "LightGreen":
                color = new Color32(0, 255, 100, 255);
            break;
            case "LightGray":
                color = new Color32(160, 160, 160, 255);
            break;
            case "Brown":
                color = new Color32(85, 45, 0, 255);
            break;
            default:
            break;
        }
        gameObject.GetComponent<Image>().color = color;
    }

    public void OnMouseEnter()
    {
        PixelCoords.text = $"( {X}, {Y} )";
    }

    public void OnMouseExit()
    {
        PixelCoords.text = "";
    }

    void Start(){
        PixelCoords = GameObject.FindGameObjectWithTag("Coords").GetComponent<Text>();
        color = gameObject.GetComponent<Image>().color;
    }
}
