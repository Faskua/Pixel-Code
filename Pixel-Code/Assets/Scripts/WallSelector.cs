using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WallSelector : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    private Wall wall;

    public void GenerateCanvas(){
        switch(dropdown.value){
            case 0:
                wall.ChangeSize(4);
            break;
            case 1:
                wall.ChangeSize(8);
            break;
            case 2:
                wall.ChangeSize(16);
            break;
            case 3:
                wall.ChangeSize(32);
            break;
            case 4:
                wall.ChangeSize(64);
            break;
            case 5:
                wall.ChangeSize(128);
            break;
            default:
                wall.ChangeSize(256);
            break;
        }

    }

    void Start(){
        wall = GameObject.FindGameObjectWithTag("WALL").GetComponent<Wall>();
    }
}
