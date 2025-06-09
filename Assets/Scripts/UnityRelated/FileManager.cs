using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class FileManager : MonoBehaviour
{
    public TMP_InputField Name;
    public TMP_InputField Code;

    public void SaveFile()
    {
        string fileName = Name.text;
        if (!fileName.EndsWith(".pw"))
        {
            fileName += ".pw";
        }
        string filePath = @"D:\Universidad\Programación\Proyectos\Pixel-Code\Pixel-Code\Assets\Codes\" + fileName;

        File.WriteAllText(filePath, Code.text);
    }

    public void LoadFile()
    {
        string fileName = Name.text;
        if (!fileName.EndsWith(".pw"))
        {
            fileName += ".pw";
        }
        string filePath = @"D:\Universidad\Programación\Proyectos\Pixel-Code\Pixel-Code\Assets\Codes\" + fileName;

        Code.text = File.ReadAllText(filePath);
    }
}
