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
            //Debug.Log(item.Type);
        }
        var statements = parser.Parse(tokens, Wall);
        foreach (var item in statements)
        {
            //Debug.Log(item.Type);
            //item.Evaluate(Global);
        }
        //Expression n = Global.GetVariable("n", new CodeLocation(1,1));
        //Debug.Log(n.Evaluate(Global));
        
        // foreach (var label in Global.Labels)
        // {
        //     string name = label.Key;
        //     List<Statement> stat = (label.Value as BlockStatement).Statements;
        //     //Debug.Log($"stat es null: {stat == null}");
        //     foreach (var statement in stat)
        //     {
        //         Debug.Log($"De la label {name}: {statement.Type}");
        //     }
        // }

        if(Global.Errors.Count != 0) Debug.Log("Hay errores por arreglar");
        else{
            int lasterror = -1,  times = 0;
            for (int index = 0; index < statements.Count; index++)
            {
                Statement statement = statements[index];
                // if(!statement.Validate(Global)){
                //     Debug.Log($"No se evaluo el {statement.Type} por un error");
                //     for(int i=lasterror+1; i < Global.Errors.Count; i++){
                //         Debug.Log(Global.Errors[i]);
                //     }
                //     lasterror = Global.Errors.Count-1;
                // }
                // else{
                    //if(statement.CheckType(IDType.GoTo)) continue;
                    Debug.Log($"Se ejecuta la statement: {statement.Type}");
                    statement.Evaluate(Global);
                    if(statement.CheckType(IDType.GoTo, Global)){
                        if ((statement as GoTo).LabelIndex != -1 && times < 4)
                        {
                            index = (statement as GoTo).LabelIndex - 1;
                            times++;
                        }
                    }
                //} 
            }
        }
        Debug.Log($"El global tiene {Global.Variables.Count} variables");
        foreach (var variable in Global.Variables)
        {
            //Debug.Log($"La variable: {variable.Key} ");
            //Debug.Log($"La variable: {variable.Key} evalua : {variable.Value.Evaluate(Global)}");
        }

        foreach (var item in Global.Errors)
        {
            Debug.Log(item);
        }
    }

    void Evaluate(Global Global, List<Statement> statements, int start){
        int times = 0;
        if(Global.Errors.Count != 0) Debug.Log("Hay errores por arreglar");
        else{
            int lasterror = -1;
            for (int index = start; index < statements.Count; index++)
            {
                Statement statement = statements[index];
                if(!statement.Validate(Global)){
                    Debug.Log($"No se evaluo el {statement.Type} por un error");
                    for(int i=lasterror+1; i < Global.Errors.Count; i++){
                        Debug.Log(Global.Errors[i]);
                    }
                    lasterror = Global.Errors.Count-1;
                }
                else{
                    statement.Evaluate(Global);
                    if(statement.CheckType(IDType.GoTo, Global)){
                        if((statement as GoTo).LabelIndex != -1 && times < 10){
                            int i = (statement as GoTo).LabelIndex - 1;
                            Evaluate(Global, statements, i);
                            times++;
                            break;
                        }
                    }
                } 
            }
        }
    }

    void Start(){
        Wall = GameObject.FindGameObjectWithTag("WALL").GetComponent<Wall>();
    }
}
