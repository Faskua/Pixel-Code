using System;
using UnityEngine;

public abstract class Expression : ASTNode
{
    public Expression(IDType type, CodeLocation location) : base(type, location){}
    public abstract object? Evaluate(Global Global);
}

public class Number : Expression
{
    public int Value { get; private set; }
    public Number(int value, CodeLocation location) : base(IDType.Numeric, location){
        Value = value;
    }
    public override bool Validate(Global Global) => Type == IDType.Numeric;
    public override object Evaluate(Global Global) => Value;
}

public class Boolean : Expression
{
    public bool Value { get; private set; }
    public Boolean(bool value, CodeLocation location) : base(IDType.Boolean, location){
        Value = value;
    }
    public override bool Validate(Global Global) => Type == IDType.Boolean;
    public override object Evaluate(Global Global) => Value;
}

public class ColorExpression : Expression
{
    public string Value { get; private set; }
    public ColorExpression(string value, CodeLocation location) : base(IDType.Color, location){
        Value = value;
    }
    public override bool Validate(Global Global) => Type == IDType.Color;
    public override object? Evaluate(Global Global) => Value;
}

public class Variable : Expression
{
    public string Name { get; private set; }
    public bool Minus { get; private set; }
    public Variable(string name, CodeLocation location) : base(IDType.Variable, location)
    {
        Name = name;
    }
    public override bool CheckType(IDType type, Global Global) => Type == type;
    public override bool Validate(Global Global) => true;
    public override object Evaluate(Global Global)
    {
        var output = Global.GetVariable(Name,Location);
        return output;
    }
}