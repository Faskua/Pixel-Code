using System;
using System.Collections.Generic;

public static class Global
{
    public static Dictionary<string, Expression> Variables { get; set; }
    public static Dictionary<string, Statement> Labels { get; set; }

    public static void Reset(){
        Variables = new Dictionary<string, Expression>();
    }
    public static void AddVariable(string name, Expression variable){
        if(Variables == null) Reset();
        if(Variables.ContainsKey(name))     Variables[name] = variable;
        else     Variables.Add(name, variable);
    }
    public static void AddTag(string name, Statement body){
        if(Labels.ContainsKey(name)) throw new Exception($"Use of an already assigned tag at line: {body.Location.Line}, column: {body.Location.Column}");
        else Labels.Add(name, body);
    }
}