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
        Global Global = new Global();
        Lexxer lexer = new Lexxer(Global);
        Parser parser = new Parser(Global);
        var tokens = lexer.Tokenize(input);
        foreach (var item in tokens)
        {
           // Debug.Log(item.Type);
        }
        var statements = parser.Parse(tokens, Wall);
        // foreach (var item in statements)
        // {
        //     Debug.Log(item.Type);
        //     //item.Evaluate(Global);
        // }
        //Expression n = Global.GetVariable("n", new CodeLocation(1,1));
        //Debug.Log(n.Evaluate(Global));
        foreach (var item in Global.Errors)
        {
            Debug.Log(item);
        }
        // foreach (var label in Global.Labels)
        // {
        //     string name = label.Key;
        //     List<Statement> stat = (label.Value as BlockStatement).Statements;
        //     Debug.Log($"stat es null: {stat == null}");
        //     foreach (var statement in stat)
        //     {
        //         Debug.Log($"De la label {name}: {statement.Type}");
        //     }
        // }
        if(Global.Errors.Count == 0){
            int lasterror = -1;
            foreach (Statement statement in statements)
            {
                if(!statement.Validate(Global)){
                    Debug.Log($"No se evaluo el {statement.Type} por un error");
                    for(int i=lasterror+1; i < Global.Errors.Count; i++){
                        Debug.Log(Global.Errors[i]);
                    }
                    lasterror = Global.Errors.Count-1;
                }
                else statement.Evaluate(Global);
            }
        }
        else Debug.Log("Hay errores por arreglar");
    }

    void Start(){
        Wall = GameObject.FindGameObjectWithTag("WALL").GetComponent<Wall>();
    }
}
