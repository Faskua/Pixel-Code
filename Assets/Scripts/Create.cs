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
        Global global = new Global();
        Lexxer lexer = new Lexxer(global);
        Parser parser = new Parser(global);
        var tokens = lexer.Tokenize(input);
        foreach (var item in tokens)
        {
           // Debug.Log(item.Type);
        }
        var statements = parser.Parse(tokens);
        foreach (var item in statements)
        {
            Debug.Log(item.Type);
            //item.Evaluate(global);
        }
        //Expression n = global.GetVariable("n", new CodeLocation(1,1));
        //Debug.Log(n.Evaluate(global));
        foreach (var item in global.Errors)
        {
            Debug.Log(item);
        }
    }

    void Start(){
        Wall = GameObject.FindGameObjectWithTag("WALL").GetComponent<Wall>();
    }
}
