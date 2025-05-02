using System;
using UnityEngine;
using System.Collections.Generic;

public  class Global
{
    //public static Wall Wall;
    public  Dictionary<string, Expression> Variables { get; set; }
    public  Dictionary<string, Statement> Labels { get; set; }
    public  List<string> Errors { get; set; }
    public Global(){
        Variables = new Dictionary<string, Expression>();
        Labels = new Dictionary<string, Statement>();
        Errors = new List<string>();
    }

    public  void AddVariable(string name, Expression variable){
        if(Variables.ContainsKey(name))     Variables[name] = variable;
        else     Variables.Add(name, variable);
    }
    public  Expression GetVariable(string name, CodeLocation location){
        if(!Variables.ContainsKey(name)){
            AddError($"Use of a not assigned variable at line: {location.Line}, column: {location.Column}");
            return null;
        } 
        return Variables[name];        
    }
    public  Statement GetLable(string name, CodeLocation location){
        if(!Labels.ContainsKey(name)){
            AddError($"Use of a not assigned label at line: {location.Line}, column: {location.Column}");
            return null;
        } 
        return Labels[name];        
    }
    public  void AddLabel(string name, Statement body){
        if(Labels.ContainsKey(name)) AddError($"Use of an already assigned label at line: {body.Location.Line}, column: {body.Location.Column}");
        else Labels.Add(name, body);
    }
    public  void AddError(string error){
        Errors.Add(error);
    }
}