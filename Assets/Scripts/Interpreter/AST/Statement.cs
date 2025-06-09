using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Statement : ASTNode
{
    public Statement(IDType type, CodeLocation location) : base(type, location){}
    public abstract void Evaluate(Global Global);
}

public class GoTo : Statement
{
    public string Label { get; private set; }
    public Expression Condition { get; private set; }
    public int LabelIndex { get; private set; }
    public GoTo(IDType type, CodeLocation location, string label, Expression condition) : base(type, location){
        Label = label;
        Condition = condition;
        LabelIndex = -1;
    }
    public override bool Validate(Global Global) => Global.Labels.ContainsKey(Label) && Condition.CheckType(IDType.Boolean, Global);
    public override void Evaluate(Global Global){
        Debug.Log("llamada a evaluar el goto");
        bool condition = (bool)Condition.Evaluate(Global);
        if(!condition){
            Debug.Log("Evalua false");
            LabelIndex = -1;
            return;
        }
        Debug.Log("Evalua true");
        LabelIndex = Global.GetLable(Label, Condition.Location);
    }
}

public class Declaration : Statement
{
    public string Name { get; private set; }
    public Expression Variable;
    public Declaration(IDType type, CodeLocation location, string name, Expression variable) : base(type, location)
    {
        Name = name;
        Variable = variable;
    }

    public override bool Validate(Global Global) => Variable.Validate(Global);
    public override void Evaluate(Global Global)
    {
        var result = Variable.Evaluate(Global);
        Debug.Log($"En la declaracion de {Name} da {result}");
        Global.AddVariable(Name, result);
    }
    
}


public class BlockStatement : Statement
{
    public List<Statement> Statements { get; set; }
    public BlockStatement(IDType type, CodeLocation location, List<Statement> statements) : base(type, location)
    {
        Statements = statements;
    }
    public override bool Validate(Global Global)
    {
        bool output = true;
        foreach (Statement statement in Statements)
        {
            output = output && statement.Validate(Global);
        }
        return output;
    }
    public override void Evaluate(Global Global)
    {
        foreach (Statement statement in Statements)
        {
            statement.Evaluate(Global);
        }
    }
}

