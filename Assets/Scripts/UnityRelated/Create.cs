using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Create : MonoBehaviour
{
    public TMP_InputField Input;
    public Text ErrorConsole;
    public Wall Wall;

    public void OnClick(){
        string input = Input.text;
        ErrorConsole.text = "";
        Global Global = new Global();
        Lexxer lexer = new Lexxer(Global);
        Parser parser = new Parser(Global);
        var tokens = lexer.Tokenize(input);
        var statements = parser.Parse(tokens, Wall);

        if(Global.Errors.Count != 0) Debug.Log("Hay errores por arreglar");
        else{
            for (int index = 0; index < statements.Count; index++)
            {
                Statement statement = statements[index];
                Debug.Log($"Se ejecuta la statement: {statement.Type}");
                statement.Evaluate(Global);
                if(statement.CheckType(IDType.GoTo, Global)){
                    if ((statement as GoTo).LabelIndex != -1)
                    {
                        index = (statement as GoTo).LabelIndex - 1;
                    }
                }
            }
        }

        foreach (var error in Global.Errors)
        {
            ErrorConsole.text += error + '\n';
        }
    }

    void Start(){
        Wall = GameObject.FindGameObjectWithTag("WALL").GetComponent<Wall>();
    }
}
