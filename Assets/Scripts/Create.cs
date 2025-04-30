using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Create : MonoBehaviour
{
    public TMP_InputField Input;
    public Wall Wall;

    public void OnClick(){
        string input = Input.text;
        Lexxer lexer = new Lexxer();
        Parser parser = new Parser();
        var tokens = lexer.Tokenize(input);
        parser.Tokens = tokens;
    }

    void Start(){
        Wall = GameObject.FindGameObjectWithTag("WALL").GetComponent<Wall>();
    }
}
