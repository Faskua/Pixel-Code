using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PixelUN : MonoBehaviour
{
    public string Color;
    private Color color;

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
                color = new Color32(6, 215, 58, 255);
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
            default:
            break;
        }
        gameObject.GetComponent<Image>().color = color;
    }

    void Start(){
        color = gameObject.GetComponent<Image>().color;
    }
}
