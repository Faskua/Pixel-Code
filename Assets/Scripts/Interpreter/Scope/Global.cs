using System;
using System.Collections.Generic;

public static class Global
{
    public static Dictionary<string, Expression> Variables { get; set; }
    public static Dictionary<string, Statement> Labels { get; set; }
    public static List<string> Errors { get; set; }

    public static void Reset(){
        Variables = new Dictionary<string, Expression>();
        Labels = new Dictionary<string, Statement>();
        Errors = new List<string>();
    }
    public static void AddVariable(string name, Expression variable){
        if(Variables == null) Reset();
        if(Variables.ContainsKey(name))     Variables[name] = variable;
        else     Variables.Add(name, variable);
    }
    public static Expression GetVariable(string name, CodeLocation location){
        if(!Variables.ContainsKey(name)){
            Errors.Add($"Use of a not assigned variable at line: {location.Line}, column: {location.Column}");
            return null;
        } 
        return Variables[name];        
    }
    public static Statement GetLable(string name, CodeLocation location){
        if(!Labels.ContainsKey(name)){
            Errors.Add($"Use of a not assigned label at line: {location.Line}, column: {location.Column}");
            return null;
        } 
        return Labels[name];        
    }
    public static void AddTag(string name, Statement body){
        if(Labels.ContainsKey(name)) Global.Errors.Add($"Use of an already assigned label at line: {body.Location.Line}, column: {body.Location.Column}");
        else Labels.Add(name, body);
    }
}