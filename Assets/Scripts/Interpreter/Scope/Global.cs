using System;
using UnityEngine;
using System.Collections.Generic;

public  class Global
{
    //public static Wall Wall;
    public  Dictionary<string, object> Variables { get; set; }
    public  Dictionary<string, int> Labels { get; set; }
    public  List<string> Errors { get; set; }
    public List<int> Lines { get; set; }
    public Global()
    {
        Variables = new Dictionary<string, object>();
        Labels = new Dictionary<string, int>();
        Errors = new List<string>();
        Lines = new List<int>();
    }

    public  void AddVariable(string name, object variable){
        if(Variables.ContainsKey(name))     Variables[name] = variable;
        else     Variables.Add(name, variable);
    }
    public object GetVariable(string name, CodeLocation location){
        if(!Variables.ContainsKey(name)){
            AddError(location.Line, $"Use of a not assigned variable at line: {location.Line}, column: {location.Column}");
            return null;
        } 
        return Variables[name];        
    }
    public int GetLable(string name, CodeLocation location){
        if(!Labels.ContainsKey(name)){
            AddError(location.Line, $"Use of a not assigned label at line: {location.Line}, column: {location.Column}");
            return -1;
        } 
        return Labels[name];        
    }
    public  void AddLabel(string name, int index, CodeLocation location){
        if(Labels.ContainsKey(name)) AddError(location.Line, $"Use of an already assigned label at line: {location.Line}, column: {location.Column}");
        else Labels.Add(name, index);
    }
    public  void AddError(int line, string error){
        Errors.Add(error);
        Lines.Add(line);
    }
}