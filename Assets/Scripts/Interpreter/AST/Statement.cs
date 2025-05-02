using System;
using System.Collections.Generic;

public abstract class Statement : ASTNode
{
    public Statement(IDType type, CodeLocation location) : base(type, location){}
    public abstract void Evaluate(Global Global);
}

public class GoTo : Statement
{
    public string Label { get; private set; }
    public Expression Condition { get; private set; }
    public GoTo(IDType type, CodeLocation location, string label, Expression condition) : base(type, location){
        Label = label;
        Condition = condition;
    }
    public override bool Validate(Global Global) => Global.Labels.ContainsKey(Label) && Condition.CheckType(IDType.Boolean);
    public override void Evaluate(Global Global){
        if(!(bool)Condition.Evaluate(Global)) return;
        Global.Labels[Label].Evaluate(Global);
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

    public override bool Validate(Global Global) => Variable.Validate(Global);
    public override void Evaluate(Global Global) => Global.AddVariable(Name, Variable);
}


public class BlockStatement : Statement
{
    public List<Statement> Statements { get; set; }
    public BlockStatement(IDType type, CodeLocation location, List<Statement> statements) : base(type, location){
        Statements = statements;
    }
    public override bool Validate(Global Global){
        bool output = true;
        foreach (Statement statement in Statements)
        {
            output = output && statement.Validate(Global);
        }
        return output;
    }
    public override void Evaluate(Global Global){
        foreach (Statement statement in Statements)
        {
            statement.Evaluate(Global);
        }
    }
}

