using System;

public abstract class Expression : ASTNode
{
    public Expression(IDType type, CodeLocation location) : base(type, location){}
    public abstract object? Evaluate();
}

public class Number : Expression
{
    public double Value { get; private set; }
    public Number(double value, CodeLocation location) : base(IDType.Numeric, location){
        Value = value;
    }
    public override bool Validate() => Type == IDType.Numeric;
    public override object Evaluate() => Value;
}

public class Boolean : Expression
{
    public bool Value { get; private set; }
    public Boolean(bool value, CodeLocation location) : base(IDType.Boolean, location){
        Value = value;
    }
    public override bool Validate() => Type == IDType.Boolean;
    public override object Evaluate() => Value;
}

public class Variable : Expression
{
    public string Name { get; private set; }
    public Expression Value { get; private set; }
    public Variable(string name, Expression value) : base(value.Type, value.Location){
        Name = name;
        Value = value;
    }
    public void ChangeValue(Expression node){
        Value = node;
    }
    public override bool Validate() => Value.Validate();
    public override object Evaluate() => Value.Evaluate();
}