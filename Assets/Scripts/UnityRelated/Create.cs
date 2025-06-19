using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public class Create : MonoBehaviour
{
    public TMP_InputField Input;
    public Text ErrorConsole;
    public Wall Wall;

    public async void OnClick()
    {
        string input = Input.text;
        ErrorConsole.text = "";
        Global Global = new Global();
        Lexxer lexer = new Lexxer(Global);
        Parser parser = new Parser(Global);
        int v = 100;
        if (Wall.Created)
        {
            var tokens = lexer.Tokenize(input);
            var statements = parser.Parse(tokens, Wall);


            for (int index = 0; index < statements.Count; index++)
            {
                if (Global.Errors.Count != 0) break;
                Statement statement = statements[index];
                statement.Evaluate(Global);
                if (statement.CheckType(IDType.GoTo, Global))
                {
                    if ((statement as GoTo).LabelIndex != -1)
                    {
                        index = (statement as GoTo).LabelIndex - 1;
                    }
                }
                switch (Wall.Size)
                {
                    case 4:
                        v = 150;
                        break;
                    case 8:
                        v = 100;
                        break;
                    case 16:
                        v = 40;
                        break;
                    case 32:
                        v = 30;
                        break;
                    case 64:
                        v = 10;
                        break;
                    case 128:
                        v = 5;
                        break;
                    default: //256
                        v = 1;
                        break;
                }
                while (Wall.paintedPixels.Count > 0)
                {
                    Wall.paintedPixels.Dequeue().Paint();
                    await Task.Delay(v);
                }
            }

        }
        else Global.AddError(0, "You need to create the wall first!!");

        foreach (var error in Global.Errors)
        {
            ErrorConsole.text += error + '\n';
        }
    }


    void Start(){
        Wall = GameObject.FindGameObjectWithTag("WALL").GetComponent<Wall>();
    }
}
