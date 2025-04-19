using System;
using UnityEngine;
using System.Collections.Generic;

public static class Global// : MonoBehavior
{
    //public static Wall Wall;
    public static Dictionary<string, Expression> Variables { get; set; }
    public static Dictionary<string, Statement> Labels { get; set; }
    public static List<string> Errors { get; set; }

    public static void AddVariable(string name, Expression variable){
        if(Variables == null) Variables = new Dictionary<string, Expression>();
        if(Variables.ContainsKey(name))     Variables[name] = variable;
        else     Variables.Add(name, variable);
    }
    public static Expression GetVariable(string name, CodeLocation location){
        if(Variables == null) Variables = new Dictionary<string, Expression>();
        if(!Variables.ContainsKey(name)){
            AddError($"Use of a not assigned variable at line: {location.Line}, column: {location.Column}");
            return null;
        } 
        return Variables[name];        
    }
    public static Statement GetLable(string name, CodeLocation location){
        if(Labels == null) Labels = new Dictionary<string, Statement>();
        if(!Labels.ContainsKey(name)){
            AddError($"Use of a not assigned label at line: {location.Line}, column: {location.Column}");
            return null;
        } 
        return Labels[name];        
    }
    public static void AddLabel(string name, Statement body){
        if(Labels == null) Labels = new Dictionary<string, Statement>();
        if(Labels.ContainsKey(name)) AddError($"Use of an already assigned label at line: {body.Location.Line}, column: {body.Location.Column}");
        else Labels.Add(name, body);
    }
    public static void AddError(string error){
        if(Errors == null) Errors = new List<string>();
        Errors.Add(error);
    }
}