using System;
using System.Collections.Generic;

public abstract class Statement : ASTNode
{
    public Statement(IDType type, CodeLocation location) : base(type, location){}
    public abstract void Evaluate();
}

public class GoTo : Statement
{
    public string Label { get; private set; }
    public Expression Condition { get; private set; }
    public GoTo(IDType type, CodeLocation location, string label, Expression condition) : base(type, location){
        Label = label;
        Condition = condition;
    }
    public override bool Validate() => Global.Labels.ContainsKey(Label) && Condition.CheckType(IDType.Boolean);
    public override void Evaluate(){
        if(!(bool)Condition.Evaluate()) return;
        Global.Labels[Label].Evaluate();
    }
}

public class Declaration : Statement
{
    public string Name { get; private set; }
    public Expression Variable { get; private set; }
    public Declaration(IDType type, CodeLocation location, string name, Expression variable) : base(type, location){
        Name = name;
        Variable = variable;
    }

    public override bool Validate(){
        if(Global.Variables.ContainsKey(Name)) throw new Exception($"Use of an already assigned variable at line: {Variable.Location.Line}, column: {Variable.Location.Column}");

        return Variable.Validate();
    }
    public override void Evaluate() => Global.AddVariable(Name, Variable);
}

public class Assignation : Statement
{
    public string Name { get; private set; }
    public Expression Variable { get; private set; }
    public Assignation(IDType type, CodeLocation location, string name, Expression variable) : base(type, location){
        Name = name;
        Variable = variable;
    }

    public override bool Validate(){
        if(!Global.Variables.ContainsKey(Name)) throw new Exception($"Not assigned variable at line: {Variable.Location.Line}, column: {Variable.Location.Column}");
        
        return Variable.Validate() && Global.Variables[Name].Type == Variable.Type;
    }
    public override void Evaluate() => Global.AddVariable(Name, Variable);
}

public class BlockStatement : Statement
{
    public List<Statement> Statements { get; set; }
    public BlockStatement(IDType type, CodeLocation location, List<Statement> statements) : base(type, location){
        Statements = statements;
    }
    public override bool Validate(){
        bool output = true;
        foreach (Statement statement in Statements)
        {
            output = output && statement.Validate();
        }
        return output;
    }
    public override void Evaluate(){
        foreach (Statement statement in Statements)
        {
            statement.Evaluate();
        }
    }
}

